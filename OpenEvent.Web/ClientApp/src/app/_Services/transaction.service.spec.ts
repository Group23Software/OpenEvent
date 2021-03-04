import { TestBed } from '@angular/core/testing';

import { TransactionService } from './transaction.service';
import {HttpClient} from "@angular/common/http";
import {UserService} from "./user.service";

describe('TransactionService', () => {
  let service: TransactionService;
  let httpClientMock;

  beforeEach(() => {

    httpClientMock = jasmine.createSpyObj('HttpClient', ['post'])

    TestBed.configureTestingModule({
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock}
      ]
    });
    service = TestBed.inject(TransactionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
