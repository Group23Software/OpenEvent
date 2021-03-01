import {Component, Input, OnInit} from '@angular/core';
import {PaymentMethodViewModel} from "../../../_models/PaymentMethod";
import {PaymentService} from "../../../_Services/payment.service";
import {HttpErrorResponse} from "@angular/common/http";
import {UserService} from "../../../_Services/user.service";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../../../_extensions/confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'virtual-card',
  templateUrl: './virtual-card.component.html',
  styleUrls: ['./virtual-card.component.css']
})
export class VirtualCardComponent implements OnInit {

  @Input() ShowActions: boolean = true;
  @Input() Card: PaymentMethodViewModel;
  public loading: boolean = false;

  constructor(private paymentService: PaymentService, private userService: UserService, private dialog: MatDialog) { }

  ngOnInit(): void {
  }

  delete ()
  {
    let deletePayment = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: 'Are you sure you want to remove this card',
        color: 'warn'
      }
    });

    deletePayment.afterClosed().subscribe(result => {
      if (result) {
        this.loading = true;
        this.paymentService.RemovePaymentMethod({
          UserId: this.userService.User.Id,
          PaymentId: this.Card.StripeCardId
        }).subscribe(response => {
          this.loading = false;
          console.log('removed');
        }, (e: HttpErrorResponse) => {
          console.error(e);
          this.loading = false;
        });
      }
    });
  }

  makeDefault ()
  {
    this.loading = true;
    this.paymentService.MakePaymentDefault({
      UserId: this.userService.User.Id,
      PaymentId: this.Card.StripeCardId
    }).subscribe(response => {
      console.log('made default');
      this.loading = false;
    }, (e: HttpErrorResponse) => {
      console.error(e);
      this.loading = false;
    });
  }
}
