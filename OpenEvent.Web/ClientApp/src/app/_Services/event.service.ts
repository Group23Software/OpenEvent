import {Inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {CreateEventBody, EventViewModel} from "../_models/Event";
import {HttpClient} from "@angular/common/http";
import {EventPaths} from "../_extensions/api.constants";
import {Category} from "../_models/Category";

@Injectable({
  providedIn: 'root'
})
export class EventService
{

  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.BaseUrl = baseUrl;
    this.BaseUrl = 'http://localhost:5000/';
  }

  public Create (createEventBody: CreateEventBody): Observable<EventViewModel>
  {
    return this.http.post<EventViewModel>(this.BaseUrl + EventPaths.BasePath, createEventBody);
  }

  public GetAllCategories (): Observable<Category[]>
  {
    return this.http.get<Category[]>(this.BaseUrl + EventPaths.GetAllCategories);
  }
}
