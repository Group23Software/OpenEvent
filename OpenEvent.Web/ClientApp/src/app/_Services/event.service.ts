import {Inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {
  CreateEventBody,
  EventDetailModel,
  EventHostModel,
  EventViewModel,
  SearchFilter,
  UpdateEventBody
} from "../_models/Event";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {EventPaths} from "../_extensions/api.constants";
import {Category} from "../_models/Category";
import {map} from "rxjs/operators";
import {SocialMedia} from "../_models/SocialMedia";
import {UserService} from "./user.service";
import {EventAnalytics, MappedEventAnalytics, PageViewEvent} from "../_models/Analytic";

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
    let params: HttpParams = new HttpParams().set('id',id);
    console.log(this.userService.User);
    if (this.userService.User != null) params = params.set('userId',this.userService.User.Id);
    return this.http.get<EventDetailModel>(this.BaseUrl + EventPaths.GetForPublic, {
      params: params
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

  public GetAllHosts (): Observable<EventHostModel[]>
  {
    return this.http.get<EventHostModel[]>(this.BaseUrl + EventPaths.GetAllHostsEvents, {params: new HttpParams().set('id', this.userService.User.Id)}).pipe(map(events =>
    {
      if (events.length > 0)
      {
        console.log('events exist', events);
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
    return this.http.post(this.BaseUrl + EventPaths.Cancel, null, {
      headers: new HttpHeaders({
        'userId': this.userService.User.Id,
        'eventId': id
      }), params: new HttpParams().set('id', id)
    });
  }

  public Search (keyword: string, filters: SearchFilter[]): Observable<EventViewModel[]>
  {
    let params: HttpParams = new HttpParams().set('keyword', keyword);
    if (this.userService.User != null) params = params.set('userId',this.userService.User.Id);
    return this.http.post<EventViewModel[]>(this.BaseUrl + EventPaths.Search, filters, {params: params});
  }

  public Explore (): Observable<EventViewModel[]>
  {
    return this.http.get<EventViewModel[]>(this.BaseUrl + EventPaths.Explore, {params: new HttpParams().set('id', this.userService.User.Id)});
  }

  public GetAnalytics (id: string): Observable<MappedEventAnalytics>
  {
    return this.http.get<EventAnalytics>(this.BaseUrl + EventPaths.Analytics, {params: new HttpParams().set('id', id)}).pipe(map(a =>
    {

      let pageViews: Map<string, PageViewEvent[]> = new Map<string, PageViewEvent[]>();
      a.PageViewEvents.forEach(x =>
      {
        let created = new Date(x.Created);
        let fullDate: string = created.toDateString();
        if (pageViews.has(fullDate))
        {
          pageViews.set(fullDate, [...pageViews.get(fullDate), x]);
        } else
        {
          pageViews.set(fullDate, [x]);
        }
      });
      console.log(pageViews);

      let mapped: MappedEventAnalytics = {
        PageViewEvents: Array.from(pageViews, ([key, value]) => ({Date: new Date(key), PageViews: value})),
        TicketVerificationEvents: a.TicketVerificationEvents,
        AverageRecommendationScores: a.AverageRecommendationScores,
        Demographics: a.Demographics
      }

      return mapped;

    }));
  }

  public DownVote (id: string): Observable<any>
  {
    return this.http.post(this.BaseUrl + EventPaths.DownVote, null, {params: new HttpParams().set('userId', this.userService.User.Id).set('eventId', id)});
  }
}
