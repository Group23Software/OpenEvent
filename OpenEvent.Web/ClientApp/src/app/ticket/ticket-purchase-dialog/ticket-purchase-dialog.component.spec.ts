import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';

import {TicketPurchaseDialogComponent, TicketPurchaseDialogData} from './ticket-purchase-dialog.component';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UserService} from "../../_Services/user.service";
import {TransactionService} from "../../_Services/transaction.service";
import {of, throwError} from "rxjs";
import {FakeEventHostModel} from "../../_testData/Event";
import {StripeService} from "ngx-stripe";
import {HttpErrorResponse} from "@angular/common/http";
import {TransactionViewModel} from "../../_models/Transaction";
import {UserAccountModel} from "../../_models/User";
import {MatStepperModule} from "@angular/material/stepper";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";


describe('TicketPurchaseDialogComponent', () =>
{
  let component: TicketPurchaseDialogComponent;
  let fixture: ComponentFixture<TicketPurchaseDialogComponent>;
  let userServiceMock;
  let transactionServiceMock;
  let dialogRefMock;
  let stripeServiceMock;
  let transaction: TransactionViewModel;

  beforeEach(async () =>
  {

    let user: UserAccountModel = {Avatar: "", Id: "UserId", IsDarkMode: false, UserName: "",PaymentMethods:[]}

    stripeServiceMock = jasmine.createSpyObj('StripeService', ['handleCardAction']);

    userServiceMock = jasmine.createSpyObj('UserService', ['NeedAccountUser'], {'User': user});
    userServiceMock.NeedAccountUser.and.returnValue(of(null));

    transaction = {
      Amount: 0,
      End: undefined,
      EventId: "",
      NextAction: undefined,
      Paid: false,
      PromoId: "",
      Start: undefined,
      Status: "",
      StripeIntentId: "IntentId",
      TicketId: "",
      Updated: undefined
    }

    transactionServiceMock = jasmine.createSpyObj('TransactionService', ['CreateIntent', 'ConfirmIntent', 'InjectPaymentMethod'], {'CurrentTransaction': transaction});
    transactionServiceMock.CreateIntent.and.returnValue(of(transaction));
    transactionServiceMock.ConfirmIntent.and.returnValue(of(transaction));
    transactionServiceMock.InjectPaymentMethod.and.returnValue(of(transaction));

    dialogRefMock = jasmine.createSpyObj('matDialogRef', ['close']);

    await TestBed.configureTestingModule({
      declarations: [TicketPurchaseDialogComponent],
      imports: [MatStepperModule,BrowserAnimationsModule],
      providers: [
        {provide: MAT_DIALOG_DATA, useValue: {Event: FakeEventHostModel} as TicketPurchaseDialogData},
        {provide: UserService, useValue: userServiceMock},
        {provide: TransactionService, useValue: transactionServiceMock},
        {provide: MatDialogRef, useValue: dialogRefMock},
        {provide: StripeService, useValue: stripeServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(TicketPurchaseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should handle create stripe intent error', () =>
  {
    transactionServiceMock.CreateIntent.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error creating intent"}})));
    component.ngOnInit();
    expect(component.loadingData).toBeFalse();
    expect(component.createIntentError).toEqual("Error creating intent");
  });

  it('should confirm intent', () =>
  {
    transactionServiceMock.ConfirmIntent.and.returnValue(of(transaction));
    component.confirm();
    expect(component.loading).toBeFalse();
    expect(dialogRefMock.disableClose).toBeTrue();
    expect(component.ticketPurchased).toBeTrue();
  });

  // it('should handle confirm intent error', fakeAsync(() =>
  // {
  //   transactionServiceMock.ConfirmIntent.and.returnValue(throwError(new HttpErrorResponse({error: {Message: 'Error confirming intent'}})));
  //   component.confirm();
  //   tick();
  //   expect(component.confirmIntentError).toEqual('Error confirming intent');
  //   expect(component.loading).toBeFalse();
  // }));

  it('should inject payment method', () =>
  {
    let stepperNextSpy = spyOn(component.stepper,'next');
    component.currentCard = {
      Brand: "",
      Country: "",
      ExpiryMonth: 0,
      ExpiryYear: 0,
      Funding: "",
      IsDefault: false,
      LastFour: "",
      Name: "",
      NickName: "",
      StripeCardId: "CardId"
    }
    component.inject();
    expect(component.stepper.steps.first.completed).toBeTrue();
    expect(stepperNextSpy).toHaveBeenCalled();
    expect(component.stepper.steps.first.editable).toBeFalse();
  });

});
