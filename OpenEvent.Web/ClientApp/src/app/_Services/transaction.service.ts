import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {TransactionPaths} from "../_extensions/api.constants";
import {
  CancelIntentBody,
  ConfirmIntentBody,
  CreateIntentBody,
  InjectPaymentMethodBody,
  TransactionViewModel
} from "../_models/Transaction";
import {map, tap} from "rxjs/operators";
import {CookieService} from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class TransactionService
{
  private readonly BaseUrl: string;

  private currentTransaction: TransactionViewModel;

  private transactionSecret: string;

  get TransactionSecret (): string
  {
    return this.transactionSecret;
  }

  get CurrentTransaction ()
  {
    return this.currentTransaction;
  }

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private cookieService: CookieService)
  {
    this.BaseUrl = baseUrl;
  }

  public CreateIntent (createIntentBody: CreateIntentBody): Observable<TransactionViewModel>
  {
    return this.http.post<TransactionViewModel>(this.BaseUrl + TransactionPaths.CreateIntent, createIntentBody).pipe(tap(t =>
    {
      this.transactionSecret = t.ClientSecret;
      t.ClientSecret = null;
      this.currentTransaction = t;
      this.cookieService.set('indent', t.StripeIntentId, new Date(new Date().getTime() + (20 * 60000)));
    }));
  }

  public InjectPaymentMethod (injectPaymentMethodBody: InjectPaymentMethodBody): Observable<TransactionViewModel>
  {
    return this.http.post<TransactionViewModel>(this.BaseUrl + TransactionPaths.InjectPaymentMethod, injectPaymentMethodBody).pipe(map(t => this.currentTransaction = t));
  }

  public ConfirmIntent (confirmIntentBody: ConfirmIntentBody): Observable<TransactionViewModel>
  {
    return this.http.post<TransactionViewModel>(this.BaseUrl + TransactionPaths.ConfirmIntent, confirmIntentBody).pipe(tap(t =>
    {
      this.currentTransaction = t;
      this.cookieService.delete('indent', '/event');
    }));
  }

  public CancelCurrentIntent (eventId: string): Observable<any>
  {
    return this.http.post(this.BaseUrl + TransactionPaths.CancelIntent, {
      Id: this.currentTransaction.StripeIntentId,
      EventId: eventId,
      TicketId: this.currentTransaction.TicketId
    } as CancelIntentBody).pipe(tap(() =>
    {
      this.currentTransaction = null;
      this.cookieService.delete('indent', '/event');
    }));
  }

  public CancelIntent (intentId: string, eventId: string, ticketId: string)
  {
    return this.http.post(this.BaseUrl + TransactionPaths.CancelIntent, {
      TicketId: ticketId,
      EventId: eventId,
      Id: intentId
    } as CancelIntentBody);
  }
}
