import {ComponentFixture, TestBed} from '@angular/core/testing';
import {AccountComponent} from "./account.component";
import {UserService} from "../_Services/user.service";
import {of, throwError} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";
import {UserAccountModel} from "../_models/User";
import {TransactionService} from "../_Services/transaction.service";
import {RouterTestingModule} from "@angular/router/testing";
import {TransactionViewModel} from "../_models/Transaction";
import {Router} from "@angular/router";
import {MatCardModule} from "@angular/material/card";
import {MatTabsModule} from "@angular/material/tabs";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";


describe('AccountComponent', () =>
{
  let component: AccountComponent;
  let fixture: ComponentFixture<AccountComponent>;
  let router;

  let userServiceMock;
  let transactionServiceMock;

  beforeEach(async () =>
  {

    userServiceMock = jasmine.createSpyObj('userService', ['GetAccountUser', 'User']);
    userServiceMock.GetAccountUser.and.returnValue(of());

    transactionServiceMock = jasmine.createSpyObj('TransactionService', ['CancelIntent'])
    transactionServiceMock.CancelIntent.and.returnValue(of());

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MatCardModule,
        MatTabsModule,
        MatProgressBarModule,
        BrowserAnimationsModule
      ],
      declarations: [AccountComponent],
      providers: [
        {provide: UserService, useValue: userServiceMock},
        {provide: TransactionService, useValue: transactionServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(AccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should get user', () =>
  {
    const user: UserAccountModel = {
      Avatar: "",
      DateOfBirth: undefined,
      Email: "",
      FirstName: "",
      Id: "",
      IsDarkMode: false,
      LastName: "",
      PhoneNumber: "",
      Tickets: [],
      Token: "",
      UserName: ""
    };
    userServiceMock.GetAccountUser.and.returnValue(of(user));
    component.ngOnInit();
    expect(component.userLoaded).toBeTrue();
  });

  it('should handle get user error', () =>
  {
    userServiceMock.GetAccountUser.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting user"}})));
    component.ngOnInit();
    expect(component.getUserError).toEqual("Error getting user");
  });

  it('should change tab', () =>
  {
    component.changeTab(2);
    expect(component.currentTab).toEqual(2);
  });

  it('should cancel transaction', () =>
  {
    let transaction: TransactionViewModel = {
      Amount: 0,
      ClientSecret: "",
      End: undefined,
      EventId: "",
      NextAction: undefined,
      Paid: false,
      PromoId: "",
      Start: undefined,
      Status: "",
      StripeIntentId: "",
      TicketId: "TicketId",
      Updated: undefined
    }
    component.cancel(transaction);
    expect(transactionServiceMock.CancelIntent).toHaveBeenCalled();
    expect(component.transactionActionLoading).toBeFalse();
  });

  it('should navigate to ticket', () =>
  {
    let routerSpy = spyOn(router, 'navigate');
    let transaction: TransactionViewModel = {
      Amount: 0,
      ClientSecret: "",
      End: undefined,
      EventId: "",
      NextAction: undefined,
      Paid: false,
      PromoId: "",
      Start: undefined,
      Status: "",
      StripeIntentId: "",
      TicketId: "TicketId",
      Updated: undefined
    }
    component.navigateToTicket(transaction);
    expect(routerSpy).toHaveBeenCalledWith(['/user/ticket/', transaction.TicketId]);
  });
});
