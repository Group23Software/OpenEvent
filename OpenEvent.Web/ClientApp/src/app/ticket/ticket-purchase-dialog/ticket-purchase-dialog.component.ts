import {Component, Inject, OnInit, ViewChild, ViewChildren} from '@angular/core';
import {PaymentMethodViewModel} from "../../_models/PaymentMethod";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {EventDetailModel} from "../../_models/Event";
import {UserService} from "../../_Services/user.service";
import {TransactionService} from "../../_Services/transaction.service";
import {HttpErrorResponse} from "@angular/common/http";
import {MatStepper} from "@angular/material/stepper";
import {StripeService} from "ngx-stripe";

export interface TicketPurchaseDialogData
{
  Event: EventDetailModel;
}

@Component({
  selector: 'app-ticket-purchase-dialog',
  templateUrl: './ticket-purchase-dialog.component.html',
  styleUrls: ['./ticket-purchase-dialog.component.css']
})
export class TicketPurchaseDialogComponent implements OnInit
{
  @ViewChild(MatStepper) stepper: MatStepper;

  currentCard: PaymentMethodViewModel;
  loadingData: boolean = false;

  createIntentError: string;
  injectPaymentMethodError: string;
  confirmIntentError: string;

  injectingPaymentMethod: boolean = false;
  confirmingTicket: boolean = false;

  ticketPurchased: boolean = false;
  loading: boolean = false;

  get UsersCards ()
  {
    return this.userService.User?.PaymentMethods;
  }

  get Event ()
  {
    return this.data.Event;
  }

  get Transaction ()
  {
    return this.transactionService.CurrentTransaction;
  }

  constructor (@Inject(MAT_DIALOG_DATA) public data: TicketPurchaseDialogData, private userService: UserService, private transactionService: TransactionService, private dialog: MatDialogRef<TicketPurchaseDialogComponent>, private stripeService: StripeService)
  {
  }

  ngOnInit (): void
  {
    this.loadingData = true;
    this.userService.NeedAccountUser().subscribe(() =>
    {
      this.currentCard = this.userService.User?.PaymentMethods?.find(x => x.IsDefault);
      this.transactionService.CreateIntent({
        Amount: this.Event.Price,
        EventId: this.Event.Id,
        UserId: this.userService.User.Id
      }).subscribe(i =>
      {
        console.log(i);
        this.loadingData = false;
      }, (e: HttpErrorResponse) =>
      {
        this.createIntentError = e.error.Message;
        this.loadingData = false;
      });
    });
  }

  public confirm (): void
  {
    this.loading = true;
    this.transactionService.ConfirmIntent({
      IntentId: this.transactionService.CurrentTransaction.StripeIntentId,
      UserId: this.userService.User.Id
    }).subscribe(i =>
    {
      if (i.Status == 'requires_action')
      {
        console.log('requires action', i);
        this.stripeService.handleCardAction(this.transactionService.TransactionSecret).subscribe(value =>
        {
          this.confirm();
        });
      } else
      {
        this.finishConfirmation();
      }
    }, (e: HttpErrorResponse) =>
    {
      this.confirmIntentError = e.error.Message;
    }, () => this.loading = false);
  }

  private finishConfirmation ()
  {
    this.dialog.disableClose = true;
    this.ticketPurchased = true;
    this.stepper.next();
    this.stepper.steps.forEach(x =>
    {
      x.completed = true;
      x.editable = false;
    });
  }

  public inject (): void
  {
    this.loading = true;
    this.transactionService.InjectPaymentMethod({
      UserId: this.userService.User.Id,
      IntentId: this.transactionService.CurrentTransaction.StripeIntentId,
      CardId: this.currentCard.StripeCardId
    }).subscribe(() =>
    {
      this.stepper.steps.first.completed = true;
      this.stepper.next();
      this.stepper.steps.first.editable = false;
    }, (e: HttpErrorResponse) => this.injectPaymentMethodError = e.message, () => this.loading = false);
  }
}
