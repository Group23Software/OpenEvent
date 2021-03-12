import {ComponentFixture, TestBed} from '@angular/core/testing';

import {EventPreviewComponent} from './event-preview.component';
import {RouterTestingModule} from "@angular/router/testing";
import {EventService} from "../../_Services/event.service";
import {Router} from "@angular/router";
import {of} from "rxjs";

describe('EventPreviewComponent', () =>
{
  let component: EventPreviewComponent;
  let fixture: ComponentFixture<EventPreviewComponent>;
  let router;

  let eventServiceMock;

  beforeEach(async () =>
  {
    eventServiceMock = jasmine.createSpyObj('EventService', ['DownVote']);
    eventServiceMock.DownVote.and.returnValue(of(null));

    await TestBed.configureTestingModule({
      declarations: [EventPreviewComponent],
      imports: [RouterTestingModule],
      providers: [
        {provide: EventService, useValue: eventServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(EventPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should navigate to event', () =>
  {
    let routerSpy = spyOn(router,'navigate');
    component.event = {
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Id: "EventId",
      IsOnline: false,
      Name: "",
      Price: 0,
      Promos: [],
      Score: 0,
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined
    }
    component.navigateToEvent();
    expect(routerSpy).toHaveBeenCalledWith(['/event',"EventId"]);
  });

  it('should down vote event', () =>
  {
    component.event = {
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Id: "EventId",
      IsOnline: false,
      Name: "",
      Price: 0,
      Promos: [],
      Score: 0,
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined
    }
    component.downVote();
    expect(eventServiceMock.DownVote).toHaveBeenCalledWith("EventId");
  });
});
