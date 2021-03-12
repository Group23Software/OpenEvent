import {TestBed} from '@angular/core/testing';

import {PromoService} from './promo.service';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {CreatePromoBody, Promo, UpdatePromoBody} from "../_models/Promo";
import {of} from "rxjs";

describe('PromoService', () =>
{
  let service: PromoService;

  let httpClientMock;

  let promoMock: Promo = {Active: false, Discount: 0, End: undefined, Id: "", NumberOfSales: 0, Start: undefined};
  let createPromoBody: CreatePromoBody = {Active: false, Discount: 0, End: undefined, EventId: "", Start: undefined};
  let updatePromoBody: UpdatePromoBody = {Active: false, Discount: 0, End: undefined, Id: "", Start: undefined};

  beforeEach(() =>
  {

    httpClientMock = jasmine.createSpyObj('HttpClient', ['post', 'delete'])

    TestBed.configureTestingModule({
      providers: [
        {provide: HttpClient, useValue: httpClientMock},
        {provide: 'BASE_URL', useValue: ''}
      ]
    });
    service = TestBed.inject(PromoService);
  });

  it('should be created', () =>
  {
    expect(service).toBeTruthy();
  });

  it('should create promo', () =>
  {
    httpClientMock.post.and.returnValue(of(promoMock));
    service.Create(createPromoBody).subscribe(x => expect(x).toEqual(promoMock));
  });

  it('should update promo', () =>
  {
    httpClientMock.post.and.returnValue(of(promoMock));
    service.Update(updatePromoBody).subscribe(x => expect(x).toEqual(promoMock));
  });

  it('should destroy promo', () =>
  {
    httpClientMock.delete.and.returnValue(of(new HttpResponse()));
    expect(service.Destroy("PromoId").subscribe()).toBeTruthy();
  });
});
