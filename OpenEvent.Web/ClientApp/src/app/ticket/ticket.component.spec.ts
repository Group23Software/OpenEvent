import {ComponentFixture, TestBed} from '@angular/core/testing';

import {TicketComponent} from './ticket.component';
import {ActivatedRoute, convertToParamMap} from "@angular/router";
import {TicketService} from "../_Services/ticket.service";
import {of} from "rxjs";
import {MatDialog} from "@angular/material/dialog";

describe('TicketComponent', () =>
{
  let component: TicketComponent;
  let fixture: ComponentFixture<TicketComponent>;

  let ticketServiceMock;
  let dialogMock;

  beforeEach(async () =>
  {
    ticketServiceMock = jasmine.createSpyObj('TicketService', ['Get'])
    ticketServiceMock.Get.and.returnValue(of(null));

    dialogMock = jasmine.createSpyObj('matDialog',['open']);

    await TestBed.configureTestingModule({
      declarations: [TicketComponent],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            snapshot: {
              paramMap: convertToParamMap({id: "1"})
            }
          }
        },
        {provide: TicketService, useValue: ticketServiceMock},
        {provide: MatDialog, useValue: dialogMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(TicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
