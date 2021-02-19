import { TestBed } from '@angular/core/testing';

import { BankingService } from './banking.service';
import {HttpClient} from "@angular/common/http";

describe('BankingService', () => {
  let service: BankingService;

  let httpClientMock;

  beforeEach(() => {

    httpClientMock = jasmine.createSpyObj('HttpClient',['post']);

    TestBed.configureTestingModule({
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock}
      ]
    });
    service = TestBed.inject(BankingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
