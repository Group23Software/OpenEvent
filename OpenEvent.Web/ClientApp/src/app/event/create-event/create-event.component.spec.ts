import {async, ComponentFixture, TestBed} from '@angular/core/testing';

import {CreateEventComponent} from './create-event.component';
import {MatDialog, MatDialogModule} from "@angular/material/dialog";
import {UserService} from "../../_Services/user.service";
import {EventService} from "../../_Services/event.service";
import {BrowserDynamicTestingModule} from "@angular/platform-browser-dynamic/testing";
import {of, throwError} from "rxjs";
import {Category} from "../../_models/Category";
import {HttpErrorResponse} from "@angular/common/http";
import {SocialMedia} from "../../_models/SocialMedia";
import {StepperSelectionEvent} from "@angular/cdk/stepper";
import {EventDetailModel} from "../../_models/Event";
import {Router} from "@angular/router";
import {RouterTestingModule} from "@angular/router/testing";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FakeAddress} from "../../_testData/Event";

describe('CreateEventComponent', () =>
{
  let component: CreateEventComponent;
  let fixture: ComponentFixture<CreateEventComponent>;
  let router;
  let dialogMock;
  let userServiceMock;
  let eventServiceMock;

  let dialogSpy: jasmine.Spy;
  let dialogRefSpyObj = jasmine.createSpyObj({afterClosed: of({}), close: null});

  beforeEach(async(() =>
  {

    dialogMock = jasmine.createSpyObj('matDialog', ['open', 'afterClosed']);

    userServiceMock = jasmine.createSpyObj('UserService', ['User','NeedAccountUser']);
    userServiceMock.NeedAccountUser.and.returnValue(of(null));

    eventServiceMock = jasmine.createSpyObj('EventService', ['GetAllCategories', 'Create']);
    eventServiceMock.GetAllCategories.and.returnValue(of());
    eventServiceMock.Create.and.returnValue(of());

    TestBed.configureTestingModule({
      declarations: [CreateEventComponent],
      imports: [
        BrowserDynamicTestingModule,
        RouterTestingModule,
        MatDialogModule,
        BrowserAnimationsModule
      ],
      providers: [
        // {provide: MatDialog, useValue: dialogMock},
        {provide: UserService, useValue: userServiceMock},
        {provide: EventService, useValue: eventServiceMock}
      ]
    }).compileComponents();
  }));

  beforeEach(() =>
  {
    router = TestBed.inject(Router);
    dialogSpy = spyOn(TestBed.inject(MatDialog), 'open').and.returnValue(dialogRefSpyObj);
    fixture = TestBed.createComponent(CreateEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  //TODO: form control validation tests

  it('should disable / enable address form if online', () =>
  {
    component.IsOnline.setValue(false);
    fixture.detectChanges();
    component.clickedOnline();
    expect(component.addressForm.disabled).toBeTruthy();
    component.IsOnline.setValue(true);
    fixture.detectChanges();
    component.clickedOnline();
    expect(component.addressForm.enabled).toBeTruthy();
  });

  it('should get all categories on init', () =>
  {
    const categories: Category[] = [
      {Id: "1", Name: "Music"},
      {Id: "2", Name: "Performance"}
    ]
    eventServiceMock.GetAllCategories.and.returnValue(of(categories));
    component.ngOnInit();
    expect(component.categoryStore).toEqual(categories);
  });

  it('should handle get all categories error', () =>
  {
    eventServiceMock.GetAllCategories.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting categories"}})));
    component.ngOnInit();
    expect(component.getError).toEqual("Error getting categories");
  });

  it('should load event data', () =>
  {
    component.addressForm.setValue({
      AddressLine1: "AddressLine1",
      AddressLine2: "AddressLine2",
      PostalCode: "PostalCode",
      CountryName: "CountryName",
      City: "City",
      CountryCode: "CountryCode",
      Lon: 1,
      Lat: 1
    });
    component.createEventForm.controls.Categories.setValue(null);
    component.createEventForm.controls.Description.setValue("Description");
    component.DateForm.controls.EndLocal.setValue(new Date(0));
    component.eventImages = null;
    component.IsOnline.setValue(true);
    component.createEventForm.controls.Name.setValue("Name");
    component.createEventForm.controls.Price.setValue(10);

    component.SocialLinks.controls.Site.setValue("Site");
    component.SocialLinks.controls.Instagram.setValue("Instagram");
    component.SocialLinks.controls.Twitter.setValue("Twitter");
    component.SocialLinks.controls.Facebook.setValue("Facebook");
    component.SocialLinks.controls.Reddit.setValue("Reddit");

    component.DateForm.controls.StartLocal.setValue(new Date(0));
    component.thumbnail = null;
    component.createEventForm.controls.NumberOfTickets.setValue(10);
    let e = new StepperSelectionEvent();
    e.selectedIndex = 3
    component.loadEventData(e);
    const eventPreview: EventDetailModel = {
      Address: {
        AddressLine1: "AddressLine1",
        AddressLine2: "AddressLine2",
        PostalCode: "PostalCode",
        CountryName: "CountryName",
        City: "City",
        CountryCode: "CountryCode",
        Lon: 1,
        Lat: 1
      },
      Categories: null,
      Description: "Description",
      EndLocal: new Date(0),
      EndUTC: undefined,
      Id: "",
      Images: null,
      IsOnline: true,
      Name: "Name",
      Price: 10,
      SocialLinks: [
        {SocialMedia: SocialMedia.Site, Link: "Site"},
        {SocialMedia: SocialMedia.Instagram, Link: "Instagram"},
        {SocialMedia: SocialMedia.Twitter, Link: "Twitter"},
        {SocialMedia: SocialMedia.Facebook, Link: "Facebook"},
        {SocialMedia: SocialMedia.Reddit, Link: "Reddit"}
      ],
      StartLocal: new Date(0),
      StartUTC: undefined,
      Thumbnail: null,
      TicketsLeft: 10
    }
    expect(component.eventPreview).toEqual(eventPreview);
  });

  it('should create event', () =>
  {
    let routerSpy = spyOn(router, 'navigate');
    eventServiceMock.Create.and.returnValue(of(true));
    component.create();
    expect(component.loading).toBeFalse();
    expect(routerSpy).toHaveBeenCalled();
  });

  it('should handle create event error', () =>
  {
    eventServiceMock.Create.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error creating event"}})));
    component.create();
    expect(component.loading).toBeFalse();
    expect(component.CreateError).toEqual("Error creating event");
  });
});
