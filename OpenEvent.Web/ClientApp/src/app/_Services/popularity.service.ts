import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {EventViewModel} from "../_models/Event";
import {PopularityPaths} from "../_extensions/api.constants";
import {CategoryViewModel} from "../_models/Category";
import {tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class PopularityService
{
  private readonly BaseUrl: string;
  private popularEvents: EventViewModel[];
  private popularCategories: CategoryViewModel[];

  get PopularEvents() {
    return this.popularEvents
  }

  get PopularCategories() {
    return this.popularCategories
  }

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.BaseUrl = baseUrl;
  }

  public GetEvents() : Observable<EventViewModel[]>
  {
    return this.http.get<EventViewModel[]>(this.BaseUrl + PopularityPaths.Events).pipe(tap(events => this.popularEvents = events));
  }

  public GetCategories() : Observable<EventViewModel[]>
  {
    return this.http.get<EventViewModel[]>(this.BaseUrl + PopularityPaths.Categories).pipe(tap(categories => this.popularCategories = categories));
  }
}
