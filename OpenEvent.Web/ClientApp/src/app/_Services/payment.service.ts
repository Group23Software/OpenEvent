import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {PaymentPaths} from "../_extensions/api.constants";
import {
  AddPaymentMethodBody,
  MakeDefaultBody,
  PaymentMethodViewModel,
  RemovePaymentMethodBody
} from "../_models/PaymentMethod";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {UserService} from "./user.service";

@Injectable({
  providedIn: 'root'
})
export class PaymentService
{
  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private userService: UserService)
  {
    this.BaseUrl = baseUrl;
    this.BaseUrl = 'http://localhost:5000/';
  }

  public AddPaymentMethod (addPaymentMethodBody: AddPaymentMethodBody): Observable<PaymentMethodViewModel>
  {
    return this.http.post<PaymentMethodViewModel>(this.BaseUrl + PaymentPaths.AddPaymentMethod, addPaymentMethodBody).pipe(map(paymentMethod =>
    {
      this.userService.User.PaymentMethods.push(paymentMethod);
      return paymentMethod;
    }));
  }

  public RemovePaymentMethod (removePaymentMethodBody: RemovePaymentMethodBody): Observable<any>
  {
    return this.http.post<any>(this.BaseUrl + PaymentPaths.RemovePaymentMethod, removePaymentMethodBody).pipe(map(x =>
    {
      this.userService.User.PaymentMethods = this.userService.User.PaymentMethods.filter(p => p.StripeCardId != removePaymentMethodBody.PaymentId)
      return x;
    }));
  }

  public MakePaymentDefault (makeDefaultBody: MakeDefaultBody)
  {
    return this.http.post<any>(this.BaseUrl + PaymentPaths.MakePaymentDefault, makeDefaultBody).pipe(map(x =>
    {
      this.userService.User.PaymentMethods.map(p => p.IsDefault = false);
      this.userService.User.PaymentMethods.find(p => p.StripeCardId == makeDefaultBody.PaymentId).IsDefault = true;
      return x;
    }));
  }
}
