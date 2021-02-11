import {ComponentFixture, TestBed} from '@angular/core/testing';

import {EventPreviewComponent} from './event-preview.component';
import {RouterTestingModule} from "@angular/router/testing";

describe('EventPreviewComponent', () =>
{
  let component: EventPreviewComponent;
  let fixture: ComponentFixture<EventPreviewComponent>;

  beforeEach(async () =>
  {
    await TestBed.configureTestingModule({
      declarations: [EventPreviewComponent],
      imports: [RouterTestingModule]
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
