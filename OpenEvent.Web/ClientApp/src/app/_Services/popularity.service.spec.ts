import { TestBed } from '@angular/core/testing';

import { PopularityService } from './popularity.service';
import {HttpClient} from "@angular/common/http";

describe('PopularityService', () => {
  let service: PopularityService;

  let httpClientMock;

  beforeEach(() => {

    httpClientMock = jasmine.createSpyObj('httpClient',['get'])

    TestBed.configureTestingModule({
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock}
      ]
    });
    service = TestBed.inject(PopularityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
