import {TestBed} from '@angular/core/testing';

import {TicketService} from './ticket.service';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {of} from "rxjs";
import {TicketDetailModel} from "../_models/Ticket";
import {UserAccountModel} from "../_models/User";
import {UserService} from "./user.service";

describe('TicketService', () =>
{
  let service: TicketService;

  let httpClientMock;
  let userServiceMock;

  beforeEach(() =>
  {
    let user: UserAccountModel = {Avatar: "", Id: "UserId", IsDarkMode: false, UserName: "",PaymentMethods:[]}

    httpClientMock = jasmine.createSpyObj('HttpClient', ['post', 'get']);

    userServiceMock = jasmine.createSpyObj('UserService', [''], {'User': user})

    TestBed.configureTestingModule({
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock},
        {provide: UserService, useValue: userServiceMock}
      ]
    });
    service = TestBed.inject(TicketService);
  });

  it('should be created', () =>
  {
    expect(service).toBeTruthy();
  });

  it('should get all users tickets', () =>
  {
    let tickets = [{
      EventEnd: undefined,
      EventId: "",
      EventName: "",
      EventStart: undefined,
      Id: "",
      QRCode: ""
    }]
    httpClientMock.get.and.returnValue(of(tickets));
    service.GetAllUsersTickets().subscribe(x => expect(x).toEqual(tickets))
  });

  it('should get ticket', () =>
  {
    let ticket: TicketDetailModel = {Event: undefined, Id: "Ticket Id", QRCode: "", Transaction: undefined};
    httpClientMock.get.and.returnValue(of(ticket));
    service.Get("TicketId").subscribe(x => expect(x).toEqual(ticket))
  });

  it('should verify ticket', () =>
  {
    httpClientMock.post.and.returnValue(of(new HttpResponse()));
    service.Verify({Id: "TicketId", EventId: "EventId"}).subscribe(x => expect(x).toEqual(new HttpResponse()));
  });

});
