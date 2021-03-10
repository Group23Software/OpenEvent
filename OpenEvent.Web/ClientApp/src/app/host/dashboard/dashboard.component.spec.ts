import {ComponentFixture, TestBed} from '@angular/core/testing';

import {DashboardComponent} from './dashboard.component';
import {of, throwError} from "rxjs";
import {EventService} from "../../_Services/event.service";
import {Router} from "@angular/router";
import {RouterTestingModule} from "@angular/router/testing";
import {MatDialog} from "@angular/material/dialog";
import {EventHostModel} from "../../_models/Event";
import {ConfirmDialogComponent} from "../../_extensions/confirm-dialog/confirm-dialog.component";
import {HttpErrorResponse} from "@angular/common/http";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";

describe('DashboardComponent', () =>
{
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  let router;
  let eventServiceMock;
  let dialogMock;

  const events = [
    {
      Address: undefined,
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Images: [],
      IsOnline: false,
      Name: "",
      Price: 0,
      SocialLinks: [],
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined,
      TicketsLeft: 0,
      Id: "2"
    },
    {
      Address: undefined,
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Images: [],
      IsOnline: false,
      Name: "",
      Price: 0,
      SocialLinks: [],
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined,
      TicketsLeft: 0,
      Id: "3"
    }
  ] as EventHostModel[]

  beforeEach(async () =>
  {

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllHosts', 'Cancel'], ['Events']);
    eventServiceMock.GetAllHosts.and.returnValue(of());

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule
      ],
      declarations: [DashboardComponent],
      providers: [
        {provide: EventService, useValue: eventServiceMock},
        {provide: MatDialog, useValue: dialogMock},
      ]
    })
                 .compileComponents();
  });

  beforeEach(() =>
  {
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    component.eventPreview = {
      Promos: [],
      Address: undefined,
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Images: [],
      IsOnline: false,
      Name: "Test event",
      Price: 0,
      SocialLinks: [],
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined,
      TicketsLeft: 0,
      Id: "1"
    }
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should navigate to config', () =>
  {
    component.eventPreview = events[0];
    spyOnProperty(component, 'Events', 'get').and.returnValue(events);
    let navigateSpy = spyOn(router, 'navigate');
    component.navigateToConfig();
    expect(navigateSpy).toHaveBeenCalledWith(['/host/config/', component.eventPreview.Id], {state: component.eventPreview})
  });

  it('should open confirm when deleting account', () =>
  {
    dialogMock.open.and.returnValue({afterClosed: () => of(null)});
    component.cancelEvent("1");
    expect(dialogMock.open).toHaveBeenCalledWith(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: `Are you sure you want to cancel "${component.eventPreview.Name}", this cannot be undone`,
        color: 'warn'
      }
    });
  });

  it('should cancel event', () =>
  {
    eventServiceMock.Cancel.and.returnValue(of(true));
    dialogMock.open.and.returnValue({afterClosed: () => of(true)});
    let routerSpy = spyOn(router, 'navigate');
    component.cancelEvent("1");
    expect(routerSpy).toHaveBeenCalledWith(['/host/events']);
  });

  it('should handle cancel event error', () =>
  {
    eventServiceMock.Cancel.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error canceling event"}})));
    dialogMock.open.and.returnValue({afterClosed: () => of(true)});
    component.cancelEvent("1");
    expect(component.cancelingEventError).toEqual("Error canceling event");
  });

  it('should get all hosts events', () =>
  {
    spyOnProperty(component, 'Events', 'get').and.returnValue(events);
    eventServiceMock.GetAllHosts.and.returnValue(of(true));
    component.ngOnInit();
    expect(component.loading).toBeFalse();
    expect(component.eventPreview).toEqual(events[0]);
  });

  it('should handle get all hosts events error', () =>
  {
    eventServiceMock.GetAllHosts.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting all hosts events"}})));
    component.ngOnInit();
    expect(component.gettingEventsError).toEqual("Error getting all hosts events");
  });

  it('should toggle preview', () =>
  {
    component.togglePreview(events[1]);
    expect(component.eventPreview).toEqual(events[1]);
  });

});
