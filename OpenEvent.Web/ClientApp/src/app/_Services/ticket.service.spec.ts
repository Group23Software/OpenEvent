import {TestBed} from '@angular/core/testing';

import {TicketService} from './ticket.service';
import {HttpClient} from "@angular/common/http";

describe('TicketService', () =>
{
  let service: TicketService;

  let httpClientMock;

  beforeEach(() =>
  {
    httpClientMock = jasmine.createSpyObj('HttpClient', ['post'])

    TestBed.configureTestingModule({
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock}
      ]
    });
    service = TestBed.inject(TicketService);
  });

  it('should be created', () =>
  {
    expect(service).toBeTruthy();
  });
});
