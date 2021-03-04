import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyTicketComponent } from './verify-ticket.component';
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute, convertToParamMap} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {TicketService} from "../../_Services/ticket.service";

describe('VerifyTicketComponent', () => {
  let component: VerifyTicketComponent;
  let fixture: ComponentFixture<VerifyTicketComponent>;
  let ticketServiceMock;
  let snackBarMock;
  let dialogMock;

  beforeEach(async () => {

    snackBarMock = jasmine.createSpyObj('matSnackBar', ['open']);
    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    ticketServiceMock = jasmine.createSpyObj('TicketService', ['Verify'])

    await TestBed.configureTestingModule({
      declarations: [ VerifyTicketComponent ],
      providers: [
        {provide: MatSnackBar, useValue: snackBarMock},
        {
          provide: ActivatedRoute, useValue: {
            snapshot: {
              paramMap: convertToParamMap({id: "1"})
            }
          }
        },
        {provide: MatDialog, useValue: dialogMock},
        {provide: TicketService, useValue: ticketServiceMock}
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
