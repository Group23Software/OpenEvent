import {TestBed} from '@angular/core/testing';

import {PaymentService} from './payment.service';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {of} from "rxjs";
import {
  AddPaymentMethodBody,
  MakeDefaultBody,
  PaymentMethodViewModel,
  RemovePaymentMethodBody
} from "../_models/PaymentMethod";
import {UserService} from "./user.service";
import {UserAccountModel} from "../_models/User";

class FakeUserService
{
  set User (value: UserAccountModel)
  {
    this._User = value;
  }

  get User (): UserAccountModel
  {
    return this._User;
  }

  private _User: UserAccountModel = {
    Avatar: "", Id: "", IsDarkMode: false, UserName: "", PaymentMethods: []
  };
}

describe('PaymentService', () =>
{
  let service: PaymentService;
  let userService: UserService;

  let httpClientMock;

  const mockPaymentMethodViewModel: PaymentMethodViewModel = {
    Brand: "",
    Country: "",
    ExpiryMonth: 0,
    ExpiryYear: 0,
    Funding: "",
    IsDefault: false,
    LastFour: "",
    Name: "",
    NickName: "",
    StripeCardId: ""
  }

  const mockAddPaymentMethodBody: AddPaymentMethodBody = {CardToken: "CardToken", NickName: "NickName", UserId: "UserId"};

  const mockRemovePaymentMethodBody: RemovePaymentMethodBody = {PaymentId: "PaymentId", UserId: "UserId"};

  const mockMakeDefaultBody: MakeDefaultBody = {PaymentId: "PaymentId", UserId: "UserId"}

  beforeEach(() =>
  {

    httpClientMock = jasmine.createSpyObj('HttpClient', ['post'])

    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock},
        {provide: UserService, useClass: FakeUserService}
      ]
    });
    service = TestBed.inject(PaymentService);
    userService = TestBed.inject(UserService);
  });

  it('should be created', () =>
  {
    expect(service).toBeTruthy();
  });

  it('should add payment method', () =>
  {
    httpClientMock.post.and.returnValue(of(mockPaymentMethodViewModel));
    service.AddPaymentMethod(mockAddPaymentMethodBody).subscribe(r =>
    {
      expect(r).toEqual(mockPaymentMethodViewModel);
      expect(userService.User.PaymentMethods).toEqual([mockPaymentMethodViewModel]);
    });
  });

  it('should remove payment method', () =>
  {
    httpClientMock.post.and.returnValue(of(new HttpResponse({status: 200})));
    service.RemovePaymentMethod(mockRemovePaymentMethodBody).subscribe(r => {
      expect(r).toEqual(new HttpResponse({status: 200}));
      expect(userService.User.PaymentMethods).toEqual([]);
    });
  });

  it('should make payment default', () =>
  {
    httpClientMock.post.and.returnValue(of(new HttpResponse({status: 200})));
    service.MakePaymentDefault(mockMakeDefaultBody).subscribe(r => {
      expect(r).toEqual(new HttpResponse({status: 200}));
      // TODO: check the array.
    });
  });
});
