import {TestBed} from '@angular/core/testing';
import {EventService} from './event.service';
import {HttpClient} from "@angular/common/http";
import {of} from "rxjs";
import {Category} from "../_models/Category";
import {CreateEventBody, EventDetailModel, EventHostModel, EventViewModel} from "../_models/Event";
import {UserService} from "./user.service";
import {UserAccountModel} from "../_models/User";
import {SocialMedia} from "../_models/SocialMedia";

class UserServiceStub
{
  set User (value: UserAccountModel)
  {
    this._User = value;
  }

  get User (): UserAccountModel
  {
    return this._User;
  }

  private _User: UserAccountModel = {Avatar: "", Id: "1", IsDarkMode: false, UserName: ""};

}

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
    Id: "this is a detailed event",
    Images: [],
    IsOnline: false,
    Name: "",
    Price: 0,
    SocialLinks: [
      {SocialMedia: SocialMedia.Site,Link: "Link"},
      {SocialMedia: SocialMedia.Instagram,Link: "Link"},
      {SocialMedia: SocialMedia.Twitter,Link: "Link"},
      {SocialMedia: SocialMedia.Facebook,Link: "Link"},
      {SocialMedia: SocialMedia.Reddit,Link: "Link"}
    ],
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
    Name: "this is a new event",
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

  const mockEventHostModel: EventHostModel = {
    Address: undefined,
    Categories: [],
    Description: "",
    EndLocal: undefined,
    EndUTC: undefined,
    Id: "Id",
    Images: [],
    IsOnline: false,
    Name: "this is a host event",
    Price: 0,
    SocialLinks: [],
    StartLocal: undefined,
    StartUTC: undefined,
    Thumbnail: undefined,
    Tickets: [],
    TicketsLeft: 0
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
        {provide: UserService, useClass: UserServiceStub}
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

  it('should map event enums', () =>
  {
    httpClientMock.get.and.returnValue(of(mockPublicEvent));
    service.GetForPublic(mockPublicEvent.Id).subscribe(e =>
    {
      expect(e).not.toBeNull();
      e.SocialLinks.forEach(s => {
        expect(s.SocialMedia).not.toBeNull();
      });
    })
  });

  it('should create event', () =>
  {
    httpClientMock.post.and.returnValue(of(mockEventViewModel));
    service.Create(mockCreateEventBody).subscribe(e =>
    {
      expect(e).not.toBeNull();
      expect(e.Name).toEqual(mockCreateEventBody.Name);
    })
  });

  it('should get all hosts events', () =>
  {
    httpClientMock.get.and.returnValue(of([mockEventHostModel, mockEventHostModel, mockEventHostModel]));
    service.GetAllHosts().subscribe(events =>
    {
      events.forEach(e =>
      {
        expect(e).not.toBeNull();
        expect(e.Name).toEqual(mockEventHostModel.Name);
      });
    });
  });

  it('should get for host', () =>
  {
    httpClientMock.get.and.returnValue(of(mockEventHostModel));
    service.GetForHost("Id").subscribe(e =>
    {
      expect(e).not.toBeNull();
      expect(e.Name).toEqual(mockEventHostModel.Name);
    });
  });

});
