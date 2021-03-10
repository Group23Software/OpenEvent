import {Component, OnInit} from '@angular/core';
import {UserService} from "../_Services/user.service";
import {HttpErrorResponse} from "@angular/common/http";
import {TransactionViewModel} from "../_models/Transaction";
import {TransactionService} from "../_Services/transaction.service";
import {Router} from "@angular/router";


@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit
{
  public getUserError: string;
  public userLoaded: boolean = false;
  public currentTab: number = 0;
  transactionColumns = ['stripeId', 'status', 'start', 'end', 'amount', 'paid', 'actions'];
  defaultDate = "0001-01-01T00:00:00";
  public transactionActionLoading: boolean = false;

  get User ()
  {
    return this.userService.User;
  }

  constructor (private userService: UserService, private transactionService: TransactionService, private router: Router)
  {
  }

  ngOnInit ()
  {
    this.userService.GetAccountUser(this.userService.User.Id).subscribe(user =>
    {
      this.userLoaded = true;
    }, (error: HttpErrorResponse) =>
    {
      this.getUserError = error.error.Message;
    });
  }

  public changeTab (tab: number)
  {
    this.currentTab = tab;
  }

  public cancel (transaction: TransactionViewModel)
  {
    this.transactionActionLoading = true;
    this.transactionService.CancelIntent(transaction.StripeIntentId, transaction.EventId, transaction.TicketId).subscribe(() =>
    {
      this.User.Transactions = this.User.Transactions.filter(x => x.StripeIntentId != transaction.StripeIntentId);
    }, () =>
    {
    }, () => this.transactionActionLoading = false);
  }

  public navigateToTicket (transaction: TransactionViewModel)
  {
    this.router.navigate(['/user/ticket/', transaction.TicketId]);
  }
}
