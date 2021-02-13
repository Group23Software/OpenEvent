import {ComponentFixture, fakeAsync, TestBed} from '@angular/core/testing';
import {EventConfigComponent} from './event-config.component';
import {RouterTestingModule} from "@angular/router/testing";
import {EventService} from "../../_Services/event.service";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar} from "@angular/material/snack-bar";
import {of, throwError} from "rxjs";
import {HarnessLoader} from "@angular/cdk/testing";
import {TestbedHarnessEnvironment} from "@angular/cdk/testing/testbed";
import {MatCardModule} from "@angular/material/card";
import {EventHostModel} from "../../_models/Event";
import {Category} from "../../_models/Category";
import {ActivatedRoute, convertToParamMap} from "@angular/router";
import {HttpErrorResponse} from "@angular/common/http";
import {SocialLinkViewModel} from "../../_models/SocialLink";
import {SocialMedia} from "../../_models/SocialMedia";
import {Address} from "../../_models/Address";

describe('EventConfigComponent', () =>
{
  let component: EventConfigComponent;
  let fixture: ComponentFixture<EventConfigComponent>;
  let loader: HarnessLoader;

  let snackBarMock;
  let dialogMock;
  let eventServiceMock;

  beforeEach(async () =>
  {
    snackBarMock = jasmine.createSpyObj('matSnackBar', ['open']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllCategories', 'GetForHost', 'Update'], ['HostsEvents']);
    eventServiceMock.GetAllCategories.and.returnValue({ subscribe: () => {} });
    eventServiceMock.GetForHost.and.returnValue(of(null));

    await TestBed.configureTestingModule({
      declarations: [EventConfigComponent],
      imports: [
        RouterTestingModule.withRoutes([]),
        MatCardModule
      ],
      providers: [
        {provide: EventService, useValue: eventServiceMock},
        {provide: MatDialog, useValue: dialogMock},
        {provide: MatSnackBar, useValue: snackBarMock},
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
    fixture = TestBed.createComponent(EventConfigComponent);
    component = fixture.componentInstance;
    component.event = {
      Address: {} as Address,
      Categories: [],
      Description: "",
      EndLocal: new Date(),
      EndUTC: new Date(),
      Id: "",
      Images: [],
      IsOnline: false,
      Name: "",
      Price: 0,
      SocialLinks: [] as SocialLinkViewModel[],
      StartLocal: new Date(),
      StartUTC: new Date(),
      Thumbnail: undefined,
      Tickets: [],
      TicketsLeft: 0
    }
    // loader = TestbedHarnessEnvironment.loader(fixture);
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should load form data', () =>
  {
    // spyOnProperty(eventServiceMock,'HostsEvents').and.returnValue([{
    //   Address: undefined,
    //   Categories: [],
    //   Description: "",
    //   EndLocal: undefined,
    //   EndUTC: undefined,
    //   Id: "test",
    //   Images: [],
    //   IsOnline: false,
    //   Name: "",
    //   Price: 0,
    //   SocialLinks: [],
    //   StartLocal: undefined,
    //   StartUTC: undefined,
    //   Thumbnail: undefined,
    //   Tickets: [],
    //   TicketsLeft: 0
    // }] as EventHostModel[]);

    console.log(eventServiceMock);

    // Object.getOwnPropertyDescriptor(eventServiceMock, "x").get.and.returnValue(7);

    // eventServiceMock.HostsEvents.get.and.returnValue([{
    //   Address: undefined,
    //   Categories: [],
    //   Description: "",
    //   EndLocal: undefined,
    //   EndUTC: undefined,
    //   Id: "test",
    //   Images: [],
    //   IsOnline: false,
    //   Name: "",
    //   Price: 0,
    //   SocialLinks: [],
    //   StartLocal: undefined,
    //   StartUTC: undefined,
    //   Thumbnail: undefined,
    //   Tickets: [],
    //   TicketsLeft: 0
    // }] as EventHostModel[]);
    // eventServiceMock.GetAllCategories.and.returnValue(of([{}] as Category[]));
    //
    // component.ngOnInit();


  });

  it('should disable address from', fakeAsync(() =>
  {
    component.IsOnline.setValue(component.IsOnline.value);
    component.clickedOnline();
    for (let control in component.addressForm.controls)
    {
      expect(component.addressForm.controls[control].disabled).toBeTrue();
    }
  }));

  it('should re-enable address form', () =>
  {
    component.clickedOnline();
    component.IsOnline.setValue(!component.IsOnline.value);
    component.clickedOnline();
    for (let control in component.addressForm.controls)
    {
      expect(component.addressForm.controls[control].enabled).toBeTrue();
    }
  });

  it('should get all categories', () =>
  {
    const categories: Category[] = [
      {Name: "Music", Id: "1"},
      {Name: "Sport", Id: "2"}
    ];
    eventServiceMock.GetAllCategories.and.returnValue(of(categories));
    component.ngOnInit();
    expect(component.categoryStore).toEqual(categories);
  });

  it('should handle category error', () =>
  {
    eventServiceMock.GetAllCategories.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting categories"}})));
    component.ngOnInit();
    expect(component.gettingCategoriesError).toEqual("Error getting categories");
  });

  it('should get event if null', () =>
  {
    const event: EventHostModel = {
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
      Tickets: [],
      TicketsLeft: 0
    }
    component.event = null;
    eventServiceMock.GetAllCategories.and.returnValue(of(true));
    eventServiceMock.GetForHost.and.returnValue(of(event));
    component.ngOnInit();
    expect(component.event).toEqual(event);
  });

  it('should load date into forms', () =>
  {

    const event: EventHostModel = {
      Address: {AddressLine1: "", AddressLine2: "", City: "", CountryCode: "", CountryName: "", PostalCode: ""},
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Id: "",
      Images: [],
      IsOnline: false,
      Name: "",
      Price: 0,
      SocialLinks: [
        {SocialMedia: SocialMedia.Site, Link: ""},
        {SocialMedia: SocialMedia.Instagram, Link: ""},
        {SocialMedia: SocialMedia.Twitter, Link: ""},
        {SocialMedia: SocialMedia.Facebook, Link: ""},
        {SocialMedia: SocialMedia.Reddit, Link: ""}
      ] as SocialLinkViewModel[],
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined,
      Tickets: [],
      TicketsLeft: 0
    }
    const categories: Category[] = [
      {Name: "Music", Id: "1"},
      {Name: "Sport", Id: "2"}
    ];
    component.event = null;
    eventServiceMock.GetAllCategories.and.returnValue(of(categories));
    eventServiceMock.GetForHost.and.returnValue(of(event));
    component.ngOnInit();

    expect(component.Name.value).toEqual(event.Name);
    expect(component.Description.value).toEqual(event.Description);
    expect(component.Price.value).toEqual(event.Price);
    expect(component.StartLocal.value).toEqual(event.StartLocal);
    expect(component.EndLocal.value).toEqual(event.EndLocal);
    expect(component.NumberOfTickets.value).toEqual(event.Tickets.length);
    // expect(component.addressForm.value).toEqual(event.Address);
  });

  it('should handle get event error', () =>
  {
    const categories: Category[] = [
      {Name: "Music", Id: "1"},
      {Name: "Sport", Id: "2"}
    ];
    component.event = null;
    eventServiceMock.GetAllCategories.and.returnValue(of(categories));
    eventServiceMock.GetForHost.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting event"}})));
    eventServiceMock.HostsEvents = null;
    component.ngOnInit();
    expect(component.gettingEventError).toEqual("Error getting event");
  });

  it('should update event', () =>
  {
    eventServiceMock.Update.and.returnValue(of(true));
    component.updateEvent();
    expect(component.updatingEvent).toBeFalse();
    expect(snackBarMock.open).toHaveBeenCalledWith('Updated event', 'close', {duration: 500});
  });

  it('should not update', () =>
  {
    eventServiceMock.Update.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error updating event"}})));
    component.updateEvent();
    expect(component.updatingEvent).toBeFalse();
    expect(component.updateEventError).toEqual("Error updating event");
  });
});
