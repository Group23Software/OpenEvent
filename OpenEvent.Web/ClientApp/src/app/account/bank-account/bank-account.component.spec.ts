import {ComponentFixture, TestBed, tick} from '@angular/core/testing';

import {BankAccountComponent} from './bank-account.component';
import {StripeElementsService, StripeIbanComponent, StripeService} from "ngx-stripe";
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
    bankingServiceMock = jasmine.createSpyObj('BankingService', ['AddBankAccount','RemoveBankAccount','GetBalance','UploadIdentityDocument','AttachFrontFile','AttachAdditionalFile']);
    bankingServiceMock.GetBalance.and.returnValue(of(null));
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

  it('should set bank complete when complete', () =>
  {
    expect(component.bankComplete).toBeFalse();
    component.onChange({bankName: "TestBank", complete: true, country: "", elementType: "iban", empty: false, error: undefined});
    expect(component.bankComplete).toBeTrue();
  });

  it('should input document', () =>
  {
    bankingServiceMock.UploadIdentityDocument.and.returnValue(of(true));
    bankingServiceMock.AttachFrontFile.and.returnValue(of(true));
    component.documentInputEvent({target:{files:[null]}});
    expect(snackBarMock.open).toHaveBeenCalledWith('Uploaded Identity Document ', 'close', {duration: 500});
    expect(component.documentLoading).toBeFalse();
  });

  it('should handle input document error', () =>
  {
    bankingServiceMock.UploadIdentityDocument.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error uploading"}})));
    component.documentInputEvent({target:{files:[null]}});
    expect(component.documentError).toEqual("Error uploading");
    expect(component.documentLoading).toBeFalse();
  });

  it('should handle attach document error', () =>
  {
    bankingServiceMock.UploadIdentityDocument.and.returnValue(of(true));
    bankingServiceMock.AttachFrontFile.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error attaching"}})));
    component.documentInputEvent({target:{files:[null]}});
    expect(component.documentError).toEqual("Error attaching");
    expect(component.documentLoading).toBeFalse();
  });

  it('should input additional document', () =>
  {
    bankingServiceMock.UploadIdentityDocument.and.returnValue(of(true));
    bankingServiceMock.AttachAdditionalFile.and.returnValue(of(true));
    component.additionalDocumentInputEvent({target:{files:[null]}});
    expect(snackBarMock.open).toHaveBeenCalledWith('Uploaded Additional Identity Document ', 'close', {duration: 500});
    expect(component.documentLoading).toBeFalse();
  });

  it('should handle input additional document error', () =>
  {
    bankingServiceMock.UploadIdentityDocument.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error uploading"}})));
    component.additionalDocumentInputEvent({target:{files:[null]}});
    expect(component.documentError).toEqual("Error uploading");
    expect(component.documentLoading).toBeFalse();
  });

  it('should handle attach additional document error', () =>
  {
    bankingServiceMock.UploadIdentityDocument.and.returnValue(of(true));
    bankingServiceMock.AttachAdditionalFile.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error attaching"}})));
    component.additionalDocumentInputEvent({target:{files:[null]}});
    expect(component.documentError).toEqual("Error attaching");
    expect(component.documentLoading).toBeFalse();
  });
});