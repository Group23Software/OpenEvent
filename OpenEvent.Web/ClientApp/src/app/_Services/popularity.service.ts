import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {PopularEventViewModel} from "../_models/Event";
import {PopularityPaths} from "../_extensions/api.constants";
import {PopularCategoryViewModel} from "../_models/Category";
import {tap} from "rxjs/operators";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class PopularityService
{
  private readonly BaseUrl: string;
  private popularEvents: PopularEventViewModel[];
  private popularCategories: PopularCategoryViewModel[];

  private connection: HubConnection;

  get PopularEvents ()
  {
    return this.popularEvents
  }

  get PopularCategories ()
  {
    return this.popularCategories
  }

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.BaseUrl = baseUrl;
    this.startConnection();
    this.connection.start().then(() => {}).catch(err => console.error('error connecting to popularity hub', err));
  }

  public ListenToEvents ()
  {
    if (this.connection)
    {
      this.connection.on('events', (events: PopularEventViewModel[]) =>
      {
        this.popularEvents = events;
      });
    }
  }

  public ListenToCategories ()
  {
    if (this.connection)
    {
      this.connection.on('categories', (categories: PopularCategoryViewModel[]) =>
      {
        this.popularCategories = categories;
      });
    }
  }

  public GetEvents (): Observable<PopularEventViewModel[]>
  {
    return this.http.get<PopularEventViewModel[]>(this.BaseUrl + PopularityPaths.Events).pipe(tap(events => this.popularEvents = events));
  }

  public GetCategories (): Observable<PopularCategoryViewModel[]>
  {
    return this.http.get<PopularCategoryViewModel[]>(this.BaseUrl + PopularityPaths.Categories).pipe(tap(categories => this.popularCategories = categories));
  }

  private startConnection ()
  {
    this.connection = new HubConnectionBuilder().withUrl(this.BaseUrl + 'popularityHub').build();
  }
}
