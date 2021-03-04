import {ComponentFixture, TestBed} from '@angular/core/testing';
import {EventConfigComponent} from './event-config.component';
import {RouterTestingModule} from "@angular/router/testing";
import {EventService} from "../../_Services/event.service";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar} from "@angular/material/snack-bar";
import {of, throwError} from "rxjs";
import {HarnessLoader} from "@angular/cdk/testing";
import {MatCardModule} from "@angular/material/card";
import {ActivatedRoute, convertToParamMap} from "@angular/router";
import {FakeEventHostModel} from "../../_testData/Event";
import {HttpErrorResponse} from "@angular/common/http";
import {Category} from "../../_models/Category";


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

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllCategories', 'GetForHost', 'Update','GetAnalytics'], {'HostsEvents': null});
    eventServiceMock.GetAnalytics.and.returnValue(of(null));
    eventServiceMock.GetAllCategories.and.returnValue({
      subscribe: () =>
      {
      }
    });
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
    component.event = FakeEventHostModel;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  // (Object.getOwnPropertyDescriptor(eventServiceMock, "HostsEvents")?.get as Spy<() => EventHostModel[]>).and.returnValue([FakeEventHostModel]);


  it('should disable address from', () =>
  {
    component.IsOnline.setValue(component.IsOnline.value);
    component.clickedOnline();
    for (let control in component.addressForm.controls)
    {
      expect(component.addressForm.controls[control].disabled).toBeTrue();
    }
  });

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

  // it('should get all categories', () =>
  // {
  //   const categories: Category[] = [
  //     {Name: "Music", Id: "1"},
  //     {Name: "Sport", Id: "2"}
  //   ];
  //   eventServiceMock.GetAllCategories.and.returnValue(of(categories));
  //   component.ngOnInit();
  //   expect(component.categoryStore).toEqual(categories);
  // });

  it('should handle category error', () =>
  {
    eventServiceMock.GetAllCategories.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting categories"}})));
    component.ngOnInit();
    expect(component.gettingCategoriesError).toEqual("Error getting categories");
  });

  // it('should get event if null', () =>
  // {
  //   component.event = null;
  //   eventServiceMock.GetAllCategories.and.returnValue(of(true));
  //   eventServiceMock.GetForHost.and.returnValue(of(FakeEventHostModel));
  //   component.ngOnInit();
  //   expect(component.event).toEqual(FakeEventHostModel);
  // });

  // it('should load data into forms', () =>
  // {
  //   const categories: Category[] = [
  //     {Name: "Music", Id: "1"},
  //     {Name: "Sport", Id: "2"}
  //   ];
  //   component.event = null;
  //   eventServiceMock.GetAllCategories.and.returnValue(of(categories));
  //   eventServiceMock.GetForHost.and.returnValue(of(FakeEventHostModel));
  //   // component.ngOnInit();
  //
  //   // expect(component.Name.value).toEqual(FakeEventHostModel.Name);
  //   // expect(component.Description.value).toEqual(FakeEventHostModel.Description);
  //   // expect(component.Price.value).toEqual(FakeEventHostModel.Price);
  //   // expect(component.StartLocal.value).toEqual(FakeEventHostModel.StartLocal);
  //   // expect(component.EndLocal.value).toEqual(FakeEventHostModel.EndLocal);
  //   // expect(component.NumberOfTickets.value).toEqual(FakeEventHostModel.Tickets.length);
  //   // expect(component.addressForm.value).toEqual(event.Address);
  // });

  it('should handle get event error', () =>
  {
    const categories: Category[] = [
      {Name: "Music", Id: "1"},
      {Name: "Sport", Id: "2"}
    ];
    component.event = null;
    eventServiceMock.GetAllCategories.and.returnValue(of(categories));
    eventServiceMock.GetForHost.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting event"}})));
    component.ngOnInit();
    expect(component.gettingEventError).toEqual("Error getting event");
  });

  // it('should update event', () =>
  // {
  //   eventServiceMock.Update.and.returnValue(of(true));
  //   component.updateEvent();
  //   expect(component.updatingEvent).toBeFalse();
  //   expect(snackBarMock.open).toHaveBeenCalledWith('Updated event', 'close', {duration: 500});
  // });
  //
  // it('should not update', () =>
  // {
  //   eventServiceMock.Update.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error updating event"}})));
  //   component.updateEvent();
  //   expect(component.updatingEvent).toBeFalse();
  //   expect(component.updateEventError).toEqual("Error updating event");
  // });
  //
  // it('should use existing event', () =>
  // {
  //   const event = FakeEventHostModel;
  //   // event.Id = "2";
  //   eventServiceMock.GetAllCategories.and.returnValue(of(true));
  //   (Object.getOwnPropertyDescriptor(eventServiceMock, "HostsEvents")?.get as Spy<() => EventHostModel[]>).and.returnValue([event]);
  //   component.ngOnInit();
  //   expect(component.event).toEqual(event);
  // });
});
