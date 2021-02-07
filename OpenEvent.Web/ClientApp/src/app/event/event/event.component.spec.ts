import {ComponentFixture, TestBed} from '@angular/core/testing';

import {EventComponent} from './event.component';
import {ActivatedRoute} from "@angular/router";
import {EventService} from "../../_Services/event.service";
import {Location} from "@angular/common";
import {of} from "rxjs";
import {RouterTestingModule} from "@angular/router/testing";
import {EventDetailModel} from "../../_models/Event";

describe('EventComponent', () =>
{
  let component: EventComponent;
  let fixture: ComponentFixture<EventComponent>;
  let activatedRouteMock;
  let eventServiceMock;
  let locationMock;

  const mockEvent: EventDetailModel = {
    Address: {
      AddressLine1: "AddressLine1",
      AddressLine2: "AddressLine2",
      City: "City",
      CountryCode: "CountryCode",
      CountryName: "CountryName",
      PostalCode: "AA1 1AA"
    },
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

  beforeEach(async () =>
  {

    activatedRouteMock = jasmine.createSpyObj('activatedRoute', ['route']);

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetForPublic']);
    eventServiceMock.GetForPublic.and.returnValue(of(null));

    locationMock = jasmine.createSpyObj('location', ['back']);


    await TestBed.configureTestingModule({
      declarations: [EventComponent],
      imports: [
        RouterTestingModule.withRoutes([])
      ],
      providers: [
        // {
        //   provide: ActivatedRoute, useValue: {params: of({id: 123})}
        // },
        {provide: EventService, useValue: eventServiceMock},
        {provide: Location, useValue: locationMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(EventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should create map link', () =>
  {
    eventServiceMock.GetForPublic.and.returnValue(of(mockEvent));
    component.ngOnInit();
    expect(component.mapLink).toEqual('https://maps.google.com/maps?q=AddressLine1,City,CountryName,AA1%201AA&t=&z=13&ie=UTF8&iwloc=&output=embed');
  });
});
