import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {UserService} from "./user.service";
import {Observable} from "rxjs";
import {TicketDetailModel, TicketViewModel} from "../_models/Ticket";
import {TicketPaths} from "../_extensions/api.constants";

@Injectable({
  providedIn: 'root'
})
export class TicketService
{
  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private userService: UserService)
  {
    this.BaseUrl = baseUrl;
  }

  public GetAllUsersTickets (): Observable<TicketViewModel[]>
  {
    return this.http.get<TicketViewModel[]>(this.BaseUrl + TicketPaths.GetAllUsersTickets, {params: new HttpParams().set('id', this.userService.User.Id)});
  }

  public Get(id: string): Observable<TicketDetailModel>
  {
    return this.http.get<TicketDetailModel>(this.BaseUrl + TicketPaths.BasePath, {params: new HttpParams().set('id',id)});
  }
}
