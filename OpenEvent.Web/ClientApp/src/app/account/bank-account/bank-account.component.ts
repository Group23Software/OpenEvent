import {Component, OnInit, ViewChild} from '@angular/core';
import {StripeIbanComponent, StripeService} from "ngx-stripe";
import {CreateTokenIbanData, StripeIbanElementOptions} from "@stripe/stripe-js";
import {UserService} from "../../_Services/user.service";
import {BankingService} from "../../_Services/banking.service";
import {HttpErrorResponse} from "@angular/common/http";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'bank-account',
  templateUrl: './bank-account.component.html',
  styleUrls: ['./bank-account.component.css']
})
export class BankAccountComponent implements OnInit {

  @ViewChild(StripeIbanComponent) bank: StripeIbanComponent;
  options: StripeIbanElementOptions = {
    supportedCountries: ['SEPA'],
    placeholderCountry: 'GB'
  };

  public addBankAccountError: string;
  public removeBankAccountError: string;

  get bankAccount() {
    return this.userService.User.BankAccounts ? this.userService.User.BankAccounts[0] : null;
  }

  constructor(private stripeService: StripeService, private userService: UserService, private bankingService: BankingService, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
  }

  public addBankAccount ()
  {
    this.stripeService.createToken(this.bank.element,{
      account_holder_name: this.userService.User.FirstName + " " + this.userService.User.LastName,
      account_holder_type: "individual",
      currency: "gbp"
    } as CreateTokenIbanData).subscribe((result) => {
      console.log(result);
      if (result.token) {
        this.bankingService.AddBankAccount({
          BankToken: result.token.id,
          UserId: this.userService.User.Id
        }).subscribe(response => {
          this.snackBar.open('Added bank account', 'close', {duration: 500});
        }, (e: HttpErrorResponse) => {
          this.addBankAccountError = e.error.Message;
        })
      }
    });
  }

  removeBankAccount ()
  {
    this.bankingService.RemoveBankAccount({
      BankId: this.bankAccount.StripeBankAccountId,
      UserId: this.userService.User.Id
    }).subscribe(response => {
      this.snackBar.open('Removed bank account', 'close', {duration: 500});
    }, (e: HttpErrorResponse) => {
      this.removeBankAccountError = e.error.Message;
    });
  }
}
