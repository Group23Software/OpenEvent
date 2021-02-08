import {AccountPreferencesComponent} from "./account-preferences.component";
import {ComponentFixture, TestBed} from "@angular/core/testing";
import {UserService} from "../../_Services/user.service";
import {MatDialog} from "@angular/material/dialog";
import {RouterTestingModule} from "@angular/router/testing";
import {Router} from "@angular/router";
import {of} from "rxjs";
import {AuthService} from "../../_Services/auth.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ImageCropperModule} from "ngx-image-cropper";


describe('AccountPreferencesComponent', () =>
{
  let component: AccountPreferencesComponent;
  let fixture: ComponentFixture<AccountPreferencesComponent>;

  let userServiceMock;
  let dialogMock;
  let router;
  let authMock;
  let snackBarMock;

  beforeEach(async () =>
  {

    snackBarMock = jasmine.createSpyObj('matSnackBar',['open']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    userServiceMock = jasmine.createSpyObj('userService', ['Destroy', 'UpdateUserName', 'UpdateAvatar', 'User']);
    // userServiceMock.GetAccountUser.and.returnValue(of());

    authMock = jasmine.createSpyObj('authService', ['IsAuthenticated', 'Login']);
    authMock.IsAuthenticated.and.returnValue(of(false));

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        ImageCropperModule,
      ],
      declarations: [AccountPreferencesComponent],
      providers: [
        {provide: UserService, useValue: userServiceMock},
        {provide: MatDialog, useValue: dialogMock},
        {provide: AuthService, useValue: authMock},
        {provide: MatSnackBar, useValue: snackBarMock},
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(AccountPreferencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
