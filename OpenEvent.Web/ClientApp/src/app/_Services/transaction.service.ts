import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {UserService} from "./user.service";
import {Observable} from "rxjs";
import {TransactionPaths} from "../_extensions/api.constants";
import {CreateIntentBody, InjectPaymentMethodBody, TransactionViewModel} from "../_models/Transaction";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class TransactionService
{
  private readonly BaseUrl: string;

  private currentTransaction: TransactionViewModel;

  get CurrentTransaction() {
    return this.currentTransaction;
  }

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private userService: UserService)
  {
    this.BaseUrl = baseUrl;
  }

  public CreateIntent (createIntentBody: CreateIntentBody): Observable<TransactionViewModel>
  {
    return this.http.post<TransactionViewModel>(this.BaseUrl + TransactionPaths.CreateIntent, createIntentBody).pipe(map(t => this.currentTransaction = t));
  }

  public ConfirmIntent (injectPaymentMethodBody: InjectPaymentMethodBody): Observable<TransactionViewModel>
  {
    return this.http.post<TransactionViewModel>(this.BaseUrl + TransactionPaths.ConfirmIntent, injectPaymentMethodBody).pipe(map(t => this.currentTransaction = t));
  }
}
