import {AccountPreferencesComponent} from "./account-preferences.component";
import {ComponentFixture, fakeAsync, TestBed, tick} from "@angular/core/testing";
import {UserService} from "../../_Services/user.service";
import {MatDialog, MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {RouterTestingModule} from "@angular/router/testing";
import {Router} from "@angular/router";
import {of} from "rxjs";
import {AuthService} from "../../_Services/auth.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ImageCropperModule} from "ngx-image-cropper";
import {ImageUploadComponent} from "../../_extensions/image-upload/image-upload.component";
import {UpdateUserNameBody, UserAccountModel} from "../../_models/User";
import {HttpResponse} from "@angular/common/http";
import {ConfirmDialogComponent} from "../../_extensions/confirm-dialog/confirm-dialog.component";


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

    snackBarMock = jasmine.createSpyObj('matSnackBar', ['open']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    userServiceMock = jasmine.createSpyObj('userService', ['Destroy', 'UpdateUserName', 'UpdateAvatar', 'User', 'UserNameExists']);
    userServiceMock.UserNameExists.and.returnValue(of(false));

    authMock = jasmine.createSpyObj('authService', ['IsAuthenticated', 'Login', 'UpdatePassword']);
    authMock.IsAuthenticated.and.returnValue(of(false));

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        ImageCropperModule,
        MatDialogModule
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

  it('should render inputs', () =>
  {
    const compiled = fixture.debugElement.nativeElement;
    const inputs = [
      compiled.querySelector('input[id="email"]'),
      compiled.querySelector('input[id="firstName"]'),
      compiled.querySelector('input[id="lastName"]'),
      compiled.querySelector('input[id="userName"]'),
      compiled.querySelector('input[id="phoneNumber"]'),
      compiled.querySelector('button[id="avatarEditButton"]'),
      compiled.querySelector('button[id="deleteUser"]')
    ];

    inputs.forEach(i => {
      expect(i).toBeTruthy();
    });

    expect(inputs).not.toBeNull();
  });

  it('should open image upload', () =>
  {
    const compiled = fixture.debugElement.nativeElement;
    const editButton = compiled.querySelector('button[id="avatarEditButton"]');
    editButton.click();
    expect(dialogMock.open).toHaveBeenCalledWith(ImageUploadComponent, {
      data: {
        height: 1,
        width: 1,
        isAvatar: true
      }
    });
  });

  it('should populate inputs with current values', fakeAsync(() =>
  {
    let user: UserAccountModel = {
      Avatar: "Avatar",
      Email: "Email",
      FirstName: "firstName",
      Id: "Id",
      IsDarkMode: false,
      LastName: "LastName",
      PhoneNumber: "PhoneNumber",
      UserName: "Username"
    }

    userServiceMock.User.and.returnValue(user);

    fixture.detectChanges();
    fixture.whenStable().then(() =>
    {

      const compiled = fixture.debugElement.nativeElement;
      const inputs = [
        {input: compiled.querySelector('input[id="email"]'), value: "Avatar"},
        {input: compiled.querySelector('input[id="firstName"]'), value: "FirstName"},
        {input: compiled.querySelector('input[id="lastName"]'), value: "LastName"},
        {input: compiled.querySelector('input[id="userName"]'), value: "UserName"},
        {input: compiled.querySelector('input[id="phoneNumber"]'), value: "PhoneNumber"},
        {input: compiled.querySelector('button[id="avatarEditButton"]')},
        {input: compiled.querySelector('button[id="deleteUser"]')}
      ];
      inputs.forEach(i =>
      {
        expect(i.input).toBeTruthy();
        i.input.value = user[i.value];
        if (i.value)
        {
          expect(i.input.value).toBe(user[i.value]);
        }
      });
      expect(inputs).not.toBeNull();
    });
  }));

  it('should validate password match', () =>
  {
    component.newPasswordForm.controls.password.setValue('Password');
    component.newPasswordForm.controls.passwordConfirm.setValue('Password');
    expect(component.newPasswordForm.controls.passwordConfirm.errors).toBeFalsy();
  });

  it('should validate password not match', () =>
  {
    component.newPasswordForm.controls.password.setValue('Password');
    component.newPasswordForm.controls.passwordConfirm.setValue('DifferentPassword');
    expect(component.newPasswordForm.controls.passwordConfirm.hasError('matches')).toBeTruthy();
  });

  it('should validate username does not exists', fakeAsync(() =>
  {
    userServiceMock.UserNameExists.and.returnValue(of(false));
    component.userName.setValue('Username');
    tick();
    expect(component.userName.errors).toBeFalsy();
  }));

  it('should validate username already exists', fakeAsync(() =>
  {
    userServiceMock.UserNameExists.and.returnValue(of(true));
    component.userName.setValue('ExistingUsername');
    tick();
    expect(component.userName.hasError('usernameExists')).toBeTruthy();
  }));

  it('should update password', () =>
  {
    authMock.UpdatePassword.and.returnValue(of(new HttpResponse({status: 200})));
    component.newPasswordForm.controls.password.setValue('Password');
    component.newPasswordForm.controls.passwordConfirm.setValue('Password');
    component.user.Email = "email@email.co.uk";

    fixture.detectChanges();

    component.UpdatePassword();
    expect(component.updatePasswordLoading).toBe(false);
    expect(snackBarMock.open).toHaveBeenCalledWith('Updated password', 'close', {duration: 500});
  });

  it('should update username', () =>
  {
    userServiceMock.UpdateUserName.and.returnValue(of({UserName: 'New Username', Id: 'Id'} as UpdateUserNameBody));
    component.userName.setValue('New Username');

    fixture.detectChanges();

    component.UpdateUserName();
    expect(component.updateUserNameLoading).toBe(false);
    expect(snackBarMock.open).toHaveBeenCalledWith('Updated username', 'close', {duration: 500});
  });

  it('should open confirm', () =>
  {
    dialogMock.open.and.returnValue({afterClosed: () => of(true)});
    userServiceMock.Destroy.and.returnValue(of(false));
    component.DeleteAccount();
    expect(dialogMock.open).toHaveBeenCalledWith(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: 'Are you sure you want to permanently delete your account, this cannot be undone',
        color: 'warn'
      }
    });
  })

  it('should delete user', () =>
  {
    let routerSpy = spyOn(router,'navigate');
    component.user.Id = "ID";
    dialogMock.open.and.returnValue({afterClosed: () => of(true)});
    userServiceMock.Destroy.and.returnValue(of(true));
    component.DeleteAccount();
    expect(dialogMock.open).toHaveBeenCalled();
    expect(routerSpy).toHaveBeenCalledWith(['/login']);
  });
});
