import {ComponentFixture, TestBed} from '@angular/core/testing';
import {EventConfigComponent} from './event-config.component';
import {RouterTestingModule} from "@angular/router/testing";
import {EventService} from "../../_Services/event.service";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar} from "@angular/material/snack-bar";
import {of} from "rxjs";

describe('EventConfigComponent', () =>
{
  let component: EventConfigComponent;
  let fixture: ComponentFixture<EventConfigComponent>;

  let snackBarMock;
  let dialogMock;
  let eventServiceMock;

  beforeEach(async () =>
  {
    snackBarMock = jasmine.createSpyObj('matSnackBar',['open']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllCategories','GetForHost','HostsEvents']);
    eventServiceMock.GetAllCategories.and.returnValue(of());
    eventServiceMock.GetForHost.and.returnValue(of());
    // eventServiceMock.Host.and.returnValue(of());

    await TestBed.configureTestingModule({
      declarations: [EventConfigComponent],
      imports: [RouterTestingModule.withRoutes([])],
      providers: [
        {provide: EventService, useValue: eventServiceMock},
        {provide: MatDialog, useValue: dialogMock},
        {provide: MatSnackBar, useValue: snackBarMock},
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(EventConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
