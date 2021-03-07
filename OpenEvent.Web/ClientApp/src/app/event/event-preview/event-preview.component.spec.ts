import {ComponentFixture, TestBed} from '@angular/core/testing';

import {EventPreviewComponent} from './event-preview.component';
import {RouterTestingModule} from "@angular/router/testing";
import {EventService} from "../../_Services/event.service";

describe('EventPreviewComponent', () =>
{
  let component: EventPreviewComponent;
  let fixture: ComponentFixture<EventPreviewComponent>;

  let eventServiceMock;

  beforeEach(async () =>
  {
    eventServiceMock = jasmine.createSpyObj('EventService', ['DownVote']);

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
    fixture = TestBed.createComponent(EventPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
