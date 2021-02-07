import {async, TestBed} from '@angular/core/testing';

import {EventService} from './event.service';
import {HttpClient} from "@angular/common/http";
import {of} from "rxjs";
import {Category} from "../_models/Category";
import {CreateEventBody, EventDetailModel, EventViewModel} from "../_models/Event";

describe('EventService', () =>
{
  let service: EventService;
  let httpClientMock;

  const mockCategories: Category[] = [
    {Id: '', Name: 'Music'},
    {Id: '', Name: 'Sport'}
  ]

  const mockPublicEvent: EventDetailModel = {
    Address: undefined,
    Categories: [],
    Description: "",
    EndLocal: undefined,
    EndUTC: undefined,
    Id: "",
    Images: [],
    IsOnline: false,
    Name: "",
    Price: 0,
    SocialLinks: [],
    StartLocal: undefined,
    StartUTC: undefined,
    Thumbnail: undefined,
    TicketsLeft: 0
  }

  const mockCreateEventBody: CreateEventBody = {
    Address: undefined,
    Categories: [],
    Description: "",
    EndLocal: undefined,
    HostId: "",
    Images: [],
    IsOnline: false,
    Name: "",
    NumberOfTickets: 0,
    Price: 0,
    SocialLinks: [],
    StartLocal: undefined,
    Thumbnail: undefined
  }

  const mockEventViewModel: EventViewModel = {
    Categories: [],
    Description: mockCreateEventBody.Description,
    EndLocal: mockCreateEventBody.EndLocal,
    EndUTC: mockCreateEventBody.EndLocal,
    Id: "1",
    IsOnline: mockCreateEventBody.IsOnline,
    Name: mockCreateEventBody.Name,
    Price: mockCreateEventBody.Price,
    StartLocal: mockCreateEventBody.StartLocal,
    StartUTC: mockCreateEventBody.StartLocal,
    Thumbnail: mockCreateEventBody.Thumbnail

  }

  beforeEach(() =>
  {

    httpClientMock = jasmine.createSpyObj('httpClient', ['post', 'get']);
    httpClientMock.post.and.returnValue(jasmine.createSpyObj("post", ["subscribe"]));
    httpClientMock.post.and.returnValue(jasmine.createSpyObj("get", ["subscribe"]));

    TestBed.configureTestingModule({
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock},
      ]
    });
    service = TestBed.inject(EventService);
  });

  it('should be created', () =>
  {
    expect(service).toBeTruthy();
  });

  it('should get all categories', () =>
  {
    httpClientMock.get.and.returnValue(of(mockCategories));
    service.GetAllCategories().subscribe(c =>
    {
      expect(c).not.toBeNull();
      expect(c).toEqual(mockCategories);
    });
  });

  it('should get event for public', () =>
  {
    httpClientMock.get.and.returnValue(of(mockPublicEvent));
    service.GetForPublic(mockPublicEvent.Id).subscribe(e =>
    {
      expect(e).not.toBeNull();
      expect(e).toEqual(mockPublicEvent);
    });
  });

  it('should create event', () =>
  {
    service.Create(mockCreateEventBody).subscribe(e => {
      expect(e).not.toBeNull();
      expect(e.Name).toEqual(mockCreateEventBody.Name);
    })
  });
});
