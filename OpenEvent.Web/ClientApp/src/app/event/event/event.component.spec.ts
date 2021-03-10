import {ComponentFixture, TestBed} from '@angular/core/testing';

import {EventComponent} from './event.component';
import {EventService} from "../../_Services/event.service";
import {Location} from "@angular/common";
import {of, throwError} from "rxjs";
import {RouterTestingModule} from "@angular/router/testing";
import {EventDetailModel} from "../../_models/Event";
import {HttpErrorResponse} from "@angular/common/http";
import {MatDialog} from "@angular/material/dialog";
import {TransactionService} from "../../_Services/transaction.service";
import {ActivatedRoute, convertToParamMap} from "@angular/router";

describe('EventComponent', () =>
{
  let component: EventComponent;
  let fixture: ComponentFixture<EventComponent>;
  let eventServiceMock;
  let locationMock;
  let dialogMock;
  let transactionServiceMock;

  const mockEvent: EventDetailModel = {
    Promos: [],
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
    transactionServiceMock = jasmine.createSpyObj('TransactionService', ['CancelIntent']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetForPublic']);
    eventServiceMock.GetForPublic.and.returnValue(of(null));

    locationMock = jasmine.createSpyObj('location', ['back']);


    await TestBed.configureTestingModule({
      declarations: [EventComponent],
      imports: [
        RouterTestingModule.withRoutes([])
      ],
      providers: [
        {provide: EventService, useValue: eventServiceMock},
        {provide: Location, useValue: locationMock},
        {provide: MatDialog, useValue: dialogMock},
        {provide: TransactionService, useValue: transactionServiceMock},
        {
          provide: ActivatedRoute, useValue: {
            snapshot: {
              paramMap: convertToParamMap({id: "1"})
            }
          }
        }
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

  it('should create map link for preview', () =>
  {
    component.EventPreview = mockEvent;
    component.ngOnInit();
    expect(component.mapLink).toEqual('https://maps.google.com/maps?q=AddressLine1,City,CountryName,AA1%201AA&t=&z=13&ie=UTF8&iwloc=&output=embed');
  });

  it('should get for public', () =>
  {
    eventServiceMock.GetForPublic.and.returnValue(of(mockEvent));
    component.ngOnInit();
    expect(component.Event).toEqual(mockEvent);
  });

  it('should handle get for public error', () =>
  {
    eventServiceMock.GetForPublic.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting for public"}})));
    component.ngOnInit();
    expect(locationMock.back).toHaveBeenCalled();
  });

  it('should use preview event', () =>
  {
    component.EventPreview = {
      Promos: [],
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
    };
    component.ngOnInit();
    expect(component.Event).toEqual(component.EventPreview);
  });

  it('should trigger onInit when changes', () =>
  {
    let onInitSpy = spyOn(component, 'ngOnInit');
    component.ngOnChanges(null);
    expect(onInitSpy).toHaveBeenCalled();
  });
});
