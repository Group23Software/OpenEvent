import {Component, OnInit, ViewChild} from '@angular/core';
import {StripeIbanComponent, StripeService} from "ngx-stripe";
import {CreateTokenIbanData, StripeIbanElementOptions} from "@stripe/stripe-js";
import {UserService} from "../../_Services/user.service";
import {BankingService} from "../../_Services/banking.service";
import {HttpErrorResponse} from "@angular/common/http";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Signal} from "../../signal/Signal";
import {Balance} from "../../_models/BankAccount";

@Component({
  selector: 'bank-account',
  templateUrl: './bank-account.component.html',
  styleUrls: ['./bank-account.component.css']
})
export class BankAccountComponent implements OnInit
{

  @ViewChild(StripeIbanComponent) bank: StripeIbanComponent;
  public options: StripeIbanElementOptions = {
    supportedCountries: ['SEPA'],
    placeholderCountry: 'GB'
  };

  public addBankAccountError: string;
  public removeBankAccountError: string;

  public balance: Balance;

  get bankAccount ()
  {
    return this.userService.User.BankAccounts ? this.userService.User.BankAccounts[0] : null;
  }

  get paymentStatus ()
  {
    if (this.userService.User.StripeAccountInfo.Requirements.pending_verification?.length > 0)
    {
      return Signal.Pending;
    }
    return this.userService.User.StripeAccountInfo?.ChargesEnabled ? Signal.Enabled : Signal.Disabled;
  }

  get payoutStatus ()
  {
    if (this.userService.User.StripeAccountInfo.Requirements.pending_verification?.length > 0)
    {
      return Signal.Pending;
    }
    return this.userService.User.StripeAccountInfo?.PayoutsEnabled ? Signal.Enabled : Signal.Disabled;
  }

  get stripeRequirements ()
  {
    return this.userService.User.StripeAccountInfo.Requirements?.currently_due;
  }

  get stripeDisabledReason ()
  {
    return this.userService.User.StripeAccountInfo.Requirements?.disabled_reason;
  }

  constructor (private stripeService: StripeService, private userService: UserService, private bankingService: BankingService, private snackBar: MatSnackBar)
  {
  }

  ngOnInit (): void
  {
    if (this.userService.User)
    {
      this.bankingService.GetBalance().subscribe(balance =>
      {
        this.balance = balance;
        console.log(this.balance);
      });
    }
  }

  public addBankAccount ()
  {
    this.stripeService.createToken(this.bank.element, {
      account_holder_name: this.userService.User.FirstName + " " + this.userService.User.LastName,
      account_holder_type: "individual",
      currency: "gbp"
    } as CreateTokenIbanData).subscribe((result) =>
    {
      console.log(result);
      if (result.token)
      {
        this.bankingService.AddBankAccount({
          BankToken: result.token.id,
          UserId: this.userService.User.Id
        }).subscribe(() =>
        {
          this.snackBar.open('Added bank account', 'close', {duration: 500});
        }, (e: HttpErrorResponse) =>
        {
          this.addBankAccountError = e.error.Message;
        });
      } else if (result.error)
      {
        this.addBankAccountError = result.error.message;
      }
    });
  }

  public removeBankAccount ()
  {
    this.bankingService.RemoveBankAccount({
      BankId: this.bankAccount.StripeBankAccountId,
      UserId: this.userService.User.Id
    }).subscribe(response =>
    {
      this.snackBar.open('Removed bank account', 'close', {duration: 500});
    }, (e: HttpErrorResponse) =>
    {
      this.removeBankAccountError = e.error.Message;
    });
  }

  public async documentInputEvent (event: any)
  {
    this.bankingService.UploadIdentityDocument(event.target.files[0]).subscribe(x =>
    {
      console.log(x);
      this.bankingService.AttachFrontFile(x.id).subscribe(a => console.log(a));
    });
  }

  public additionalDocumentInputEvent (event: any)
  {
    this.bankingService.UploadIdentityDocument(event.target.files[0]).subscribe(x =>
    {
      console.log(x);
      this.bankingService.AttachAdditionalFile(x.id).subscribe(a => console.log(a));
    });
  }
}
