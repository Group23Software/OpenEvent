import {Component, OnInit, ViewChild} from '@angular/core';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';

import {StripeService, StripeCardComponent} from 'ngx-stripe';
import {
  CreateTokenCardData, StripeCardCvcElementOptions, StripeCardElementChangeEvent,
  StripeCardElementOptions, StripeCardExpiryElementOptions, StripeCardNumberElementOptions,
  StripeElementsOptions
} from '@stripe/stripe-js';
import {PaymentService} from "../../_Services/payment.service";
import {UserService} from "../../_Services/user.service";
import {HttpErrorResponse} from "@angular/common/http";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'payment-methods',
  templateUrl: './payment-methods.component.html',
  styleUrls: ['./payment-methods.component.css']
})
export class PaymentMethodsComponent implements OnInit
{
  get User ()
  {
    return this.userService.User;
  }

  public createCardTokenError: string;
  public createCardTokenLoading: boolean = false;
  public cardComplete: boolean = false;

  @ViewChild(StripeCardComponent) card: StripeCardComponent;

  cardOptions: StripeCardElementOptions = {
    style: {
      base: {
        iconColor: '#666EE8',
        color: '#31325F',
        fontWeight: '300',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSize: '18px',
        '::placeholder': {
          color: '#CFD7E0'
        }
      }
    },
    hidePostalCode: true,
  };

  cardNumberOptions: StripeCardNumberElementOptions = {
    showIcon: true,
    style: {
      base: {
        iconColor: '#666EE8',
        color: '#31325F',
        fontWeight: '300',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSize: '18px',
        '::placeholder': {
          color: '#CFD7E0'
        }
      }
    }
  };

  cardExpiryOptions: StripeCardExpiryElementOptions = {
    style: {
      base: {
        iconColor: '#666EE8',
        color: '#31325F',
        fontWeight: '300',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSize: '18px',
        '::placeholder': {
          color: '#CFD7E0'
        }
      }
    }
  }

  cardCvcOptions: StripeCardCvcElementOptions = {
    style: {
      base: {
        iconColor: '#666EE8',
        color: '#31325F',
        fontWeight: '300',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSize: '18px',
        '::placeholder': {
          color: '#CFD7E0'
        }
      }
    }
  }

  elementsOptions: StripeElementsOptions = {
    locale: 'auto'
  };

  stripeTest: FormGroup;

  constructor (
    private fb: FormBuilder,
    private stripeService: StripeService,
    private paymentService: PaymentService,
    private userService: UserService,
    private snackBar: MatSnackBar)
  {
  }

  ngOnInit (): void
  {
    this.stripeTest = this.fb.group({
      nickName: ['', [Validators.required]]
    });
  }

  onChange (ev: StripeCardElementChangeEvent)
  {
    this.cardComplete = ev.complete;
  }

  createToken (): void
  {
    this.createCardTokenLoading = true;
    console.log(this.card.state);

    if (this.User.Address == null)
    {
      this.createCardTokenError = 'User must have address';
      this.createCardTokenLoading = false;
      return;
    }

    this.stripeService.createToken(this.card.element, {
      name: this.User.FirstName + " " + this.User.LastName,
      address_line1: this.User.Address.AddressLine1,
      address_line2: this.User.Address.AddressLine2,
      address_city: this.User.Address.City,
      address_country: this.User.Address.CountryCode,
      address_zip: this.User.Address.PostalCode
    } as CreateTokenCardData)
        .subscribe((result) =>
        {
          if (result.token)
          {
            this.paymentService.AddPaymentMethod({
              CardToken: result.token.id,
              UserId: this.userService.User.Id,
              NickName: this.stripeTest.get('nickName').value
            }).subscribe(response =>
            {
              this.createCardTokenLoading = false;
              this.snackBar.open('Added payment method', 'close', {duration: 500});
              this.stripeTest.reset();
              // TODO: clear card input after added.
            }, (e: HttpErrorResponse) =>
            {
              console.error(e);
              this.createCardTokenError = e.error.Message;
              this.createCardTokenLoading = false;
            });
          } else if (result.error)
          {
            console.log(result.error.message);
            this.createCardTokenError = result.error.message;
            this.createCardTokenLoading = false;
          }
        });
  }

}
