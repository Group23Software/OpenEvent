import {Component, Inject, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {PaymentMethodViewModel} from "../../_models/PaymentMethod";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {EventDetailModel} from "../../_models/Event";
import {UserService} from "../../_Services/user.service";
import {TransactionService} from "../../_Services/transaction.service";
import {HttpErrorResponse} from "@angular/common/http";

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
  currentCard: PaymentMethodViewModel;
  loadingData: boolean = false;
  createIntentError: string;
  confirmIntentError: string;
  confirmingTicket: boolean = false;

  get Event ()
  {
    return this.data.Event;
  }

  constructor (@Inject(MAT_DIALOG_DATA) public data: TicketPurchaseDialogData, private userService: UserService, private transactionService: TransactionService, private dialog: MatDialogRef<TicketPurchaseDialogComponent>)
  {
  }

  ngOnInit (): void
  {
    this.loadingData = true;
    this.userService.NeedAccountUser().subscribe(() =>
    {
      this.currentCard =  this.userService.User?.PaymentMethods?.find(x => x.IsDefault);
      this.transactionService.CreateIntent({
        Amount: this.Event.Price,
        EventId: this.Event.Id,
        UserId: this.userService.User.Id
      }).subscribe(i => {
        console.log(i);
        this.loadingData = false;
      },(e: HttpErrorResponse) => {
        this.createIntentError = e.error.Message;
        this.loadingData = false;
      });
    });
  }

  public confirm (): void
  {
    this.confirmingTicket = true;
    this.transactionService.ConfirmIntent({
      CardId: this.currentCard.StripeCardId,
      IntentId: this.transactionService.CurrentTransaction.StripeIntentId,
      UserId: this.userService.User.Id
    }).subscribe(i => {
      console.log(i);
      this.confirmingTicket = false;
      this.dialog.close();
    },(e: HttpErrorResponse) => {
      this.confirmIntentError = e.error.Message;
      this.confirmingTicket = false;
    });
  }
}
