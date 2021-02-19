import {TestBed} from '@angular/core/testing';

import {PaymentService} from './payment.service';
import {HttpClient} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

describe('PaymentService', () =>
{
  let service: PaymentService;

  let httpClientMock;

  beforeEach(() =>
  {

    httpClientMock = jasmine.createSpyObj('HttpClient', ['post'])

    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock}
      ]
    });
    service = TestBed.inject(PaymentService);
  });

  it('should be created', () =>
  {
    expect(service).toBeTruthy();
  });
});
