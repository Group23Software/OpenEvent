import {ComponentFixture, TestBed} from '@angular/core/testing';
import {AccountComponent} from "./account.component";
import {UserService} from "../_Services/user.service";
import {of, throwError} from "rxjs";
import {MatDialog} from "@angular/material/dialog";
import {HttpErrorResponse} from "@angular/common/http";
import {UserAccountModel} from "../_models/User";


describe('AccountComponent', () =>
{
  let component: AccountComponent;
  let fixture: ComponentFixture<AccountComponent>;

  let userServiceMock;


  beforeEach(async () =>
  {

    userServiceMock = jasmine.createSpyObj('userService', ['GetAccountUser', 'User']);
    userServiceMock.GetAccountUser.and.returnValue(of());

    await TestBed.configureTestingModule({
      imports: [],
      declarations: [AccountComponent],
      providers: [
        {provide: UserService, useValue: userServiceMock},

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
