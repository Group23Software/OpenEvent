import { TestBed } from '@angular/core/testing';

import { PromoService } from './promo.service';
import {HttpClient} from "@angular/common/http";

describe('PromoService', () => {
  let service: PromoService;

  let httpClientMock;

  beforeEach(() => {

    httpClientMock = jasmine.createSpyObj('HttpClient', ['post','delete'])

    TestBed.configureTestingModule({
      providers: [
        {provide: HttpClient, useValue: httpClientMock},
        {provide: 'BASE_URL', useValue: ''}
      ]
    });
    service = TestBed.inject(PromoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
