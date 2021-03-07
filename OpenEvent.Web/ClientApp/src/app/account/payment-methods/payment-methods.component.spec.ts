import {ComponentFixture, TestBed} from '@angular/core/testing';

import {PaymentMethodsComponent} from './payment-methods.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {StripeCardComponent, StripeElementsService, StripeService} from "ngx-stripe";
import {UserService} from "../../_Services/user.service";
import {BankingService} from "../../_Services/banking.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {PaymentService} from "../../_Services/payment.service";
import {of, throwError} from "rxjs";
import {UserAccountModel} from "../../_models/User";
import {FakeAddress} from "../../_testData/Event";
import {HttpErrorResponse} from "@angular/common/http";
import {TriggerService} from "../../_Services/trigger.service";
import {IteratorStatus} from "../../_extensions/iterator/iterator.component";

describe('PaymentMethodsComponent', () =>
{
  let component: PaymentMethodsComponent;
  let fixture: ComponentFixture<PaymentMethodsComponent>;

  let stripeServiceMock;
  let paymentServiceMock;
  let userServiceMock;
  let triggerMock;

  beforeEach(async () =>
  {
    triggerMock = jasmine.createSpyObj('triggerService', ['Iterate'])
    stripeServiceMock = jasmine.createSpyObj('StripeService', ['createToken']);
    paymentServiceMock = jasmine.createSpyObj('PaymentService', ['AddPaymentMethod', 'RemovePaymentMethod']);
    userServiceMock = jasmine.createSpyObj('UserService', ['User']);

    // let userSpy = spyOnProperty(component,'User','get').and.returnValue(null);

    await TestBed.configureTestingModule({
      declarations: [PaymentMethodsComponent],
      imports: [FormsModule, ReactiveFormsModule],
      providers: [
        {provide: StripeService, useValue: stripeServiceMock},
        {provide: UserService, useValue: userServiceMock},
        {provide: PaymentService, useValue: paymentServiceMock},
        {provide: TriggerService, useValue: triggerMock},
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(PaymentMethodsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should add payment method', () =>
  {
    let service = new StripeElementsService(stripeServiceMock);
    component.card = new StripeCardComponent(service);
    stripeServiceMock.createToken.and.returnValue(of({
      token: {id: "Test card token"}
    }));
    spyOnProperty(component, 'User', 'get').and.returnValue({
      Address: FakeAddress,
      PaymentMethods: [],
      FirstName: "Test",
      LastName: "Name"
    });
    paymentServiceMock.AddPaymentMethod.and.returnValue(of(true));
    component.createToken();
    expect(triggerMock.Iterate).toHaveBeenCalledWith('Added payment method', 1000, IteratorStatus.good);
    expect(component.createCardTokenLoading).toBeFalse();
  });

  it('should handle add payment method error', () =>
  {
    let service = new StripeElementsService(stripeServiceMock);
    component.card = new StripeCardComponent(service);
    stripeServiceMock.createToken.and.returnValue(of({
      token: {id: "Test card token"}
    }));
    spyOnProperty(component, 'User', 'get').and.returnValue({
      Address: FakeAddress,
      PaymentMethods: [],
      FirstName: "Test",
      LastName: "Name"
    });
    paymentServiceMock.AddPaymentMethod.and.returnValue(throwError({error:{Message: "Error adding payment method"}} as HttpErrorResponse));
    component.createToken();
    expect(component.createCardTokenError).toEqual("Error adding payment method");
  });

  it('should handle stripe create token error', () =>
  {
    let service = new StripeElementsService(stripeServiceMock);
    component.card = new StripeCardComponent(service);
    stripeServiceMock.createToken.and.returnValue(of({
      error: {message: "Error creating card token"}
    }));
    spyOnProperty(component, 'User', 'get').and.returnValue({
      Address: FakeAddress,
      PaymentMethods: [],
      FirstName: "Test",
      LastName: "Name"
    });
    component.createToken();
    expect(component.createCardTokenError).toEqual("Error creating card token");
  });

  it('should set card complete when complete', () =>
  {
    expect(component.cardComplete).toBeFalse();
    component.onChange({
      brand: undefined,
      elementType: "card",
      empty: false,
      error: undefined,
      value: {postalCode: ""},
      complete: true});
    expect(component.cardComplete).toBeTrue();
  });
});
