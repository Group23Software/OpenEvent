import { TestBed } from '@angular/core/testing';

import { BankingService } from './banking.service';
import {HttpBackend, HttpClient} from "@angular/common/http";

describe('BankingService', () => {
  let service: BankingService;

  let httpClientMock;
  let httpBackendMock;

  beforeEach(() => {

    httpClientMock = jasmine.createSpyObj('HttpClient',['post']);

    httpBackendMock = jasmine.createSpyObj('HttpBackend', ['']);

    TestBed.configureTestingModule({
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock},
        {provide: HttpBackend, useValue: httpBackendMock}
      ]
    });
    service = TestBed.inject(BankingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
