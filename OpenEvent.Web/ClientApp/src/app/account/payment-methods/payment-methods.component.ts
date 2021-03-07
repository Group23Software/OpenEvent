import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {StripeCardComponent, StripeService} from 'ngx-stripe';
import {
  CreateTokenCardData,
  StripeCardCvcElementOptions,
  StripeCardElementChangeEvent,
  StripeCardElementOptions,
  StripeCardExpiryElementOptions,
  StripeCardNumberElementOptions,
  StripeElementsOptions
} from '@stripe/stripe-js';
import {PaymentService} from "../../_Services/payment.service";
import {UserService} from "../../_Services/user.service";
import {HttpErrorResponse} from "@angular/common/http";
import {TriggerService} from "../../_Services/trigger.service";
import {IteratorStatus} from "../../_extensions/iterator/iterator.component";

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
        iconColor: '#8966e8',
        color: '#41315f',
        // fontWeight: '300',
        fontFamily: '"Open Sans", serif',
        fontSize: '18px',
        '::placeholder': {
          color: '#676775'
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
    private trigger: TriggerService)
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
            }).subscribe(() =>
            {
              this.createCardTokenLoading = false;
              this.trigger.Iterate('Added payment method', 1000, IteratorStatus.good);
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
