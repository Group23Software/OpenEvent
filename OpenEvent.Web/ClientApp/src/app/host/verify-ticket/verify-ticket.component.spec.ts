import {ComponentFixture, TestBed} from '@angular/core/testing';

import {VerifyTicketComponent} from './verify-ticket.component';
import {ActivatedRoute, convertToParamMap} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {TicketService} from "../../_Services/ticket.service";
import {TriggerService} from "../../_Services/trigger.service";
import {of, throwError} from "rxjs";
import {IteratorStatus} from "../../_extensions/iterator/iterator.component";
import {HttpErrorResponse} from "@angular/common/http";

describe('VerifyTicketComponent', () =>
{
  let component: VerifyTicketComponent;
  let fixture: ComponentFixture<VerifyTicketComponent>;
  let ticketServiceMock;
  let triggerServiceMock;
  let dialogMock;

  beforeEach(async () =>
  {

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);
    dialogMock.open.and.returnValue({afterClosed: () => of(true)});

    ticketServiceMock = jasmine.createSpyObj('TicketService', ['Verify']);
    ticketServiceMock.Verify.and.returnValue(of(null));

    triggerServiceMock = jasmine.createSpyObj('TriggerService',['IterateForever']);
    triggerServiceMock.IterateForever.and.returnValue(of(null));

    await TestBed.configureTestingModule({
      declarations: [VerifyTicketComponent],
      providers: [
        {provide: TriggerService, useValue: triggerServiceMock},
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
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(VerifyTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should verify ticket', () =>
  {
    ticketServiceMock.Verify.and.returnValue(of(null));
    component.Id.setValue('TicketId');
    component.Verify();
    expect(triggerServiceMock.IterateForever).toHaveBeenCalledWith('Verified Ticket', IteratorStatus.good);
    expect(component.loading).toBeFalse();
  });

  it('should verify ticket from scan', () =>
  {
    ticketServiceMock.Verify.and.returnValue(of(null));
    component.scanSuccess('TicketId');
    expect(triggerServiceMock.IterateForever).toHaveBeenCalledWith('Verified Ticket', IteratorStatus.good);
    expect(component.loading).toBeFalse();
  });

  it('should not double scan', () =>
  {
    ticketServiceMock.Verify.and.returnValue(of(null));
    component.scanSuccess('TicketId');
    component.scanSuccess('TicketId');
    expect(triggerServiceMock.IterateForever).toHaveBeenCalledOnceWith('Verified Ticket', IteratorStatus.good);
    expect(component.loading).toBeFalse();
  });

  it('should handle verify error when scanning', () =>
  {
    ticketServiceMock.Verify.and.returnValue(throwError(new HttpErrorResponse({})));
    component.scanSuccess('TicketId');
    expect(dialogMock.open).toHaveBeenCalled();
    expect(component.loading).toBeFalse();
  });

  it('should handle verify error', () =>
  {
    ticketServiceMock.Verify.and.returnValue(throwError(new HttpErrorResponse({})));
    component.Id.setValue('TicketId');
    component.Verify();
    expect(dialogMock.open).toHaveBeenCalled();
    expect(component.loading).toBeFalse();
  });
});
