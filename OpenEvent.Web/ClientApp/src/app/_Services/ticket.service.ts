import {Inject, Injectable} from '@angular/core';
import {HttpBackend, HttpClient, HttpHeaders, HttpParams, HttpResponse} from "@angular/common/http";
import {UserService} from "./user.service";
import {Observable} from "rxjs";
import {TicketDetailModel, TicketViewModel, VerifyTicketBody} from "../_models/Ticket";
import {TicketPaths} from "../_extensions/api.constants";
import {Balance} from "../_models/BankAccount";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class TicketService
{
  private stripeHttp: HttpClient;
  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private userService: UserService, backend: HttpBackend)
  {
    this.BaseUrl = baseUrl;
    this.stripeHttp = new HttpClient(backend);
  }

  public GetAllUsersTickets (): Observable<TicketViewModel[]>
  {
    return this.http.get<TicketViewModel[]>(this.BaseUrl + TicketPaths.GetAllUsersTickets, {params: new HttpParams().set('id', this.userService.User.Id)});
  }

  public Get (id: string): Observable<TicketDetailModel>
  {
    return this.http.get<TicketDetailModel>(this.BaseUrl + TicketPaths.BasePath, {params: new HttpParams().set('id', id)});
  }

  public Verify (verifyTicketBody: VerifyTicketBody): Observable<any>
  {
    return this.http.post<any>(this.BaseUrl + TicketPaths.Verify, verifyTicketBody);
  }

  public GetCharge (id: string): Observable<any>
  {
    return this.stripeHttp.get<any>('https://api.stripe.com/v1/charges/' + id, {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + environment.StripeAPIKey
      })
    });
  }
}
