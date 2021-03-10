import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketReceiptDialogComponent } from './ticket-receipt-dialog.component';

describe('TicketReceiptDialogComponent', () => {
  let component: TicketReceiptDialogComponent;
  let fixture: ComponentFixture<TicketReceiptDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TicketReceiptDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketReceiptDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
