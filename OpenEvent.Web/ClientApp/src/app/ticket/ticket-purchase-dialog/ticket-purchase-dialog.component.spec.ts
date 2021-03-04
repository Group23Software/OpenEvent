import {ComponentFixture, TestBed} from '@angular/core/testing';

import {TicketPurchaseDialogComponent, TicketPurchaseDialogData} from './ticket-purchase-dialog.component';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {UserService} from "../../_Services/user.service";
import {TransactionService} from "../../_Services/transaction.service";
import {of} from "rxjs";
import {FakeEventHostModel} from "../../_testData/Event";

describe('TicketPurchaseDialogComponent', () =>
{
  let component: TicketPurchaseDialogComponent;
  let fixture: ComponentFixture<TicketPurchaseDialogComponent>;
  let userServiceMock;
  let transactionServiceMock;
  let dialogRefMock;

  beforeEach(async () =>
  {
    userServiceMock = jasmine.createSpyObj('UserService', ['NeedAccountUser']);
    userServiceMock.NeedAccountUser.and.returnValue(of(null));

    transactionServiceMock = jasmine.createSpyObj('TransactionService', ['CreateIntent','ConfirmIntent','InjectPaymentMethod']);
    transactionServiceMock.CreateIntent.and.returnValue(of(null));

    dialogRefMock = jasmine.createSpyObj('matDialogRef', ['close']);

    await TestBed.configureTestingModule({
      declarations: [TicketPurchaseDialogComponent],
      providers: [
        {provide: MAT_DIALOG_DATA, useValue: {Event: FakeEventHostModel} as TicketPurchaseDialogData},
        {provide: UserService, useValue: userServiceMock},
        {provide: TransactionService, useValue: transactionServiceMock},
        {provide: MatDialogRef, useValue: dialogRefMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(TicketPurchaseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
