import {Inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {CreatePromoBody, Promo, UpdatePromoBody} from "../_models/Promo";
import {HttpClient, HttpParams} from "@angular/common/http";
import {PromoPaths} from "../_extensions/api.constants";

@Injectable({
  providedIn: 'root'
})
export class PromoService
{

  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.BaseUrl = baseUrl;
  }

  public Create (createPromoBody: CreatePromoBody): Observable<Promo>
  {
    return this.http.post<Promo>(this.BaseUrl + PromoPaths.BasePath, createPromoBody);
  }

  public Update (updatePromoBody: UpdatePromoBody): Observable<Promo>
  {
    return this.http.post<Promo>(this.BaseUrl + PromoPaths.Update, updatePromoBody);
  }

  public Destroy (id: string): Observable<any>
  {
    return this.http.delete(this.BaseUrl + PromoPaths.BasePath, {params: new HttpParams().set('id', id)});
  }
}
