import {Inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {CreateEventBody, EventDetailModel, EventHostModel, EventViewModel, UpdateEventBody} from "../_models/Event";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {EventPaths} from "../_extensions/api.constants";
import {Category} from "../_models/Category";
import {map} from "rxjs/operators";
import {SocialMedia} from "../_models/SocialMedia";
import {UserService} from "./user.service";

@Injectable({
  providedIn: 'root'
})
export class EventService
{
  private hostsEvents: EventHostModel[];

  get HostsEvents ()
  {
    return this.hostsEvents;
  }

  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private userService: UserService)
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

  public GetForPublic (id: string): Observable<EventDetailModel>
  {
    return this.http.get<EventDetailModel>(this.BaseUrl + EventPaths.GetForPublic, {params: new HttpParams().set('id', id)}).pipe(map(e =>
    {
      let socialLinks = e.SocialLinks;
      socialLinks.map(s =>
      {
        s.SocialMedia = SocialMedia[s.SocialMedia] as SocialMedia;
        return s;
      });
      return e;
    }));
  }

  public GetAllHosts (): Observable<EventHostModel[]>
  {
    return this.http.get<EventHostModel[]>(this.BaseUrl + EventPaths.GetAllHostsEvents, {params: new HttpParams().set('id', this.userService.User.Id)}).pipe(map(events =>
    {
      if (events.length > 0)
      {
        console.log('events exist',events);
        this.hostsEvents = events;

        this.hostsEvents.sort((a, b) =>
        {
          if (a.StartLocal < b.StartLocal)
          {
            return -1;
          }
          if (a.StartLocal > b.StartLocal)
          {
            return 1;
          }
          return 0;
        });
      }
      return events;
    }));
  }

  public GetForHost (id: string): Observable<EventHostModel>
  {
    return this.http.get<EventHostModel>(this.BaseUrl + EventPaths.GetForHost, {
      params: new HttpParams().set('id', id),
      headers: new HttpHeaders({
        'userId': this.userService.User.Id,
        'eventId': id
      })
    }).pipe(map(e =>
    {
      let socialLinks = e.SocialLinks;
      socialLinks.map(s =>
      {
        s.SocialMedia = SocialMedia[s.SocialMedia] as SocialMedia;
        return s;
      });
      return e;
    }));
  }

  public Update (updateEventBody: UpdateEventBody): Observable<any>
  {
    return this.http.post<any>(this.BaseUrl + EventPaths.Update, updateEventBody, {
      headers: new HttpHeaders({
        'userId': this.userService.User.Id,
        'eventId': updateEventBody.Id
      })
    });
  }

  public Cancel (id: string): Observable<any>
  {
    return this.http.post(this.BaseUrl + EventPaths.Cancel, null,{
      headers: new HttpHeaders({
        'userId': this.userService.User.Id,
        'eventId': id
      }),params: new HttpParams().set('id',id)
    });
  }
}
