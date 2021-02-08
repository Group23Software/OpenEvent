import {Inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {CreateEventBody, EventDetailModel, EventViewModel} from "../_models/Event";
import {HttpClient, HttpParams} from "@angular/common/http";
import {EventPaths} from "../_extensions/api.constants";
import {Category} from "../_models/Category";
import {map} from "rxjs/operators";
import {SocialMedia} from "../_models/SocialMedia";

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
}
