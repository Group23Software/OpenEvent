import {ComponentFixture, TestBed} from '@angular/core/testing';

import {TicketReceiptDialogComponent} from './ticket-receipt-dialog.component';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";

describe('TicketReceiptDialogComponent', () =>
{
  let component: TicketReceiptDialogComponent;
  let fixture: ComponentFixture<TicketReceiptDialogComponent>;

  beforeEach(async () =>
  {
    await TestBed.configureTestingModule({
      declarations: [TicketReceiptDialogComponent],
      providers: [{
        provide: MAT_DIALOG_DATA, useValue: {}},
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(TicketReceiptDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
