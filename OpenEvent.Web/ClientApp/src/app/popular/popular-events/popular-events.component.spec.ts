import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopularEventsComponent } from './popular-events.component';
import {MatCardModule} from "@angular/material/card";
import {MatIconModule} from "@angular/material/icon";
import {MatProgressBarModule} from "@angular/material/progress-bar";

describe('PopularEventsComponent', () => {
  let component: PopularEventsComponent;
  let fixture: ComponentFixture<PopularEventsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        MatCardModule,
        MatIconModule,
        MatProgressBarModule
      ],
      declarations: [ PopularEventsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PopularEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
