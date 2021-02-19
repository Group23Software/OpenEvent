import {ComponentFixture, TestBed} from '@angular/core/testing';

import {BankAccountComponent} from './bank-account.component';
import {NgxStripeModule, StripeElementsService, StripeIbanComponent, StripeService} from "ngx-stripe";
import {UserService} from "../../_Services/user.service";
import {BankingService} from "../../_Services/banking.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {of, throwError} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";

describe('BankAccountComponent', () =>
{
  let component: BankAccountComponent;
  let fixture: ComponentFixture<BankAccountComponent>;

  let stripeServiceMock;
  let bankingServiceMock;
  let userServiceMock;
  let snackBarMock;

  beforeEach(async () =>
  {

    snackBarMock = jasmine.createSpyObj('matSnackBar', ['open']);
    stripeServiceMock = jasmine.createSpyObj('StripeService', ['createToken']);
    bankingServiceMock = jasmine.createSpyObj('BankingService', ['AddBankAccount','RemoveBankAccount']);
    userServiceMock = jasmine.createSpyObj('UserService', ['User']);

    userServiceMock.User = {
      get: m => m.returnValue(null),
      set: null
    }

    await TestBed.configureTestingModule({
      declarations: [BankAccountComponent],
      // imports: [NgxStripeModule],
      providers: [
        {provide: StripeService, useValue: stripeServiceMock},
        {provide: UserService, useValue: userServiceMock},
        {provide: BankingService, useValue: bankingServiceMock},
        {provide: MatSnackBar, useValue: snackBarMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(BankAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should add bank account', () =>
  {
    let service = new StripeElementsService(stripeServiceMock);
    component.bank = new StripeIbanComponent(service);
    stripeServiceMock.createToken.and.returnValue(of({
      token: {id: "Test bank account token"}
    }));
    bankingServiceMock.AddBankAccount.and.returnValue(of(true));
    component.addBankAccount();
    expect(snackBarMock.open).toHaveBeenCalledWith('Added bank account', 'close', {duration: 500});
  });

  it('should handle add bank account error', () =>
  {
    let service = new StripeElementsService(stripeServiceMock);
    component.bank = new StripeIbanComponent(service);
    stripeServiceMock.createToken.and.returnValue(of({
      token: {id: "Test bank account token"}
    }));
    bankingServiceMock.AddBankAccount.and.returnValue(throwError({error:{Message: "Error adding bank account"}} as HttpErrorResponse));
    component.addBankAccount();
    expect(component.addBankAccountError).toEqual("Error adding bank account");
  });

  it('should handle stripe create token error', () =>
  {
    let service = new StripeElementsService(stripeServiceMock);
    component.bank = new StripeIbanComponent(service);
    stripeServiceMock.createToken.and.returnValue(of({
      error: {message: "Error creating bank token"}
    }));
    component.addBankAccount();
    expect(component.addBankAccountError).toEqual("Error creating bank token");
  });

  it('should remove bank account', () =>
  {
    spyOnProperty(component,'bankAccount','get').and.returnValue({StripeBankAccountId: "Id"});
    bankingServiceMock.RemoveBankAccount.and.returnValue(of(true));
    component.removeBankAccount();
    expect(snackBarMock.open).toHaveBeenCalledWith('Removed bank account', 'close', {duration: 500});
  });

  it('should handle remove bank account error', () =>
  {
    spyOnProperty(component,'bankAccount','get').and.returnValue({StripeBankAccountId: "Id"});
    bankingServiceMock.RemoveBankAccount.and.returnValue(throwError({error: {Message: "Error removing bank account"}} as HttpErrorResponse));
    component.removeBankAccount();
    expect(component.removeBankAccountError).toEqual("Error removing bank account");
  });
});
