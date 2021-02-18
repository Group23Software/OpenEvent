import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {PaymentPaths} from "../_extensions/api.constants";
import {AddPaymentMethodModel, PaymentMethodViewModel} from "../_models/PaymentMethod";
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

  public AddPaymentMethod (addPaymentMethodModel: AddPaymentMethodModel): Observable<PaymentMethodViewModel>
  {
    return this.http.post<PaymentMethodViewModel>(this.BaseUrl + PaymentPaths.AddPaymentMethod,addPaymentMethodModel).pipe(map(paymentMethod => {
      this.userService.User.PaymentMethods.push(paymentMethod);
      return paymentMethod;
    }));
  }
}
