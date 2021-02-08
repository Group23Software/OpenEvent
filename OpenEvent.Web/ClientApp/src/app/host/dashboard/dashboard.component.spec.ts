import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardComponent } from './dashboard.component';
import {of} from "rxjs";
import {EventService} from "../../_Services/event.service";
import {Router} from "@angular/router";
import {RouterTestingModule} from "@angular/router/testing";
import {MatDialog} from "@angular/material/dialog";

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  let router;
  let eventServiceMock;
  let dialogMock;

  beforeEach(async () => {

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllHosts']);
    eventServiceMock.GetAllHosts.and.returnValue(of());

    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [ DashboardComponent ],
      providers: [
        {provide: EventService, useValue: eventServiceMock},
        {provide: MatDialog, useValue: dialogMock},
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
