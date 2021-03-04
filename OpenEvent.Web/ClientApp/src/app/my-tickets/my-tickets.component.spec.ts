import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyTicketsComponent } from './my-tickets.component';
import {TicketService} from "../_Services/ticket.service";
import {RouterTestingModule} from "@angular/router/testing";
import {of} from "rxjs";

describe('MyTicketsComponent', () => {
  let component: MyTicketsComponent;
  let fixture: ComponentFixture<MyTicketsComponent>;

  let ticketServiceMock;

  beforeEach(async () => {

    ticketServiceMock = jasmine.createSpyObj('TicketService', ['GetAllUsersTickets']);
    ticketServiceMock.GetAllUsersTickets.and.returnValue(of(null));

    await TestBed.configureTestingModule({
      declarations: [ MyTicketsComponent ],
      imports: [RouterTestingModule],
      providers: [
        {provide: TicketService, useValue: ticketServiceMock}
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyTicketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
