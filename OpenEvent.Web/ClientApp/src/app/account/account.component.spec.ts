import {ComponentFixture, TestBed} from '@angular/core/testing';
import {AccountComponent} from "./account.component";
import {UserService} from "../_Services/user.service";
import {of, throwError} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";
import {UserAccountModel} from "../_models/User";
import {TransactionService} from "../_Services/transaction.service";
import {RouterTestingModule} from "@angular/router/testing";


describe('AccountComponent', () =>
{
  let component: AccountComponent;
  let fixture: ComponentFixture<AccountComponent>;

  let userServiceMock;
  let transactionServiceMock;

  beforeEach(async () =>
  {

    userServiceMock = jasmine.createSpyObj('userService', ['GetAccountUser', 'User']);
    userServiceMock.GetAccountUser.and.returnValue(of());

    transactionServiceMock = jasmine.createSpyObj('TransactionService', ['CancelIntent'])

    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [AccountComponent],
      providers: [
        {provide: UserService, useValue: userServiceMock},
        {provide: TransactionService, useValue: transactionServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
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
    const user: UserAccountModel ={
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
});
