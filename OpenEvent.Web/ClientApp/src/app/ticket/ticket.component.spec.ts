import {ComponentFixture, TestBed} from '@angular/core/testing';

import {TicketComponent} from './ticket.component';
import {ActivatedRoute, convertToParamMap} from "@angular/router";
import {TicketService} from "../_Services/ticket.service";
import {of} from "rxjs";

describe('TicketComponent', () =>
{
  let component: TicketComponent;
  let fixture: ComponentFixture<TicketComponent>;
  let ticketServiceMock;

  beforeEach(async () =>
  {
    ticketServiceMock = jasmine.createSpyObj('TicketService', ['Get'])
    ticketServiceMock.Get.and.returnValue(of(null));

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
        {provide: TicketService, useValue: ticketServiceMock}
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
