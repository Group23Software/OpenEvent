import {async, ComponentFixture, fakeAsync, TestBed, tick} from "@angular/core/testing";
import {CreateAccountComponent} from "./create-account.component";
import {Router} from "@angular/router";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatCardModule} from "@angular/material/card";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {LoadingComponent} from "../../loading/loading.component";
import {ImageCropperModule} from "ngx-image-cropper";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {RouterTestingModule} from "@angular/router/testing";
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {CookieService} from "ngx-cookie-service";
import {UserService} from "../../_Services/user.service";
import {of, throwError} from "rxjs";
import {MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatNativeDateModule} from "@angular/material/core";
import {BrowserDynamicTestingModule} from "@angular/platform-browser-dynamic/testing";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatInputModule} from "@angular/material/input";
import {UserValidatorsService} from "../../_Services/user-validators.service";
import {UserViewModel} from "../../_models/User";
import {HttpErrorResponse} from "@angular/common/http";
import 'jasmine';

export class DialogRef
{
  public close ()
  {
  };
}

describe('CreateAccountComponent', () =>
{

  let component: CreateAccountComponent;
  let fixture: ComponentFixture<CreateAccountComponent>;
  let router;
  let userServiceMock;
  let userValidatorsServiceMock;
  let dialogRefMock;

  beforeEach(async(() =>
  {
    userValidatorsServiceMock = jasmine.createSpyObj('UserValidatorsService', ['phoneValidator', 'emailValidator', 'usernameValidator']);
    userValidatorsServiceMock.emailValidator.and.returnValue(of(null));
    userValidatorsServiceMock.phoneValidator.and.returnValue(of(null));
    userValidatorsServiceMock.usernameValidator.and.returnValue(of(null));

    userServiceMock = jasmine.createSpyObj('userService', ['CreateUser', 'EmailExists', 'PhoneExists', 'UserNameExists']);
    userServiceMock.CreateUser.and.returnValue(of());

    dialogRefMock = jasmine.createSpyObj('matDialogRef', ['close']);

    TestBed.configureTestingModule({
      declarations: [
        CreateAccountComponent,
        LoadingComponent,
      ], imports: [
        FormsModule,
        ReactiveFormsModule,
        MatCheckboxModule,
        MatFormFieldModule,
        BrowserDynamicTestingModule,
        BrowserAnimationsModule,
        MatInputModule,
        MatCardModule,
        MatDatepickerModule,
        ImageCropperModule,
        MatProgressSpinnerModule,
        RouterTestingModule,
        MatDialogModule,
        HttpClientTestingModule,
        MatNativeDateModule,
      ], providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: UserService, useValue: userServiceMock},
        {provide: MatDialogRef, useValue: dialogRefMock},
        // {provide: UserValidatorsService, useValue: userValidatorsServiceMock},
        UserValidatorsService,
        CookieService,
      ]
    }).compileComponents();
  }));

  beforeEach(() =>
    {
      router = TestBed.inject(Router);
      fixture = TestBed.createComponent(CreateAccountComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }
  );

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should set instance variables', () =>
  {
    expect(component.maxDate).not.toBeNull();
    expect(component.DefaultProfile).not.toBeNull();
  });

  it('should render input elements', async(() =>
  {
    const compiled = fixture.debugElement.nativeElement;
    const inputs = [
      compiled.querySelector('input[formControlName="email"]'),
      compiled.querySelector('input[formControlName="password"]'),
      compiled.querySelector('input[formControlName="firstName"]'),
      compiled.querySelector('input[formControlName="lastName"]'),
      compiled.querySelector('input[formControlName="userName"]'),
      compiled.querySelector('input[formControlName="phoneNumber"]'),
      compiled.querySelector('input[formControlName="dOB"]'),
      compiled.querySelector('input[id="avatar"]'),
      compiled.querySelector('mat-checkbox[formControlName="remember"]')
    ];

    inputs.forEach(input =>
    {
      expect(input).toBeTruthy();
    });
  }));

  describe('form validation', () =>
  {
    it('form should be valid', fakeAsync(() =>
    {
      userServiceMock.EmailExists.and.returnValue(of(false));
      userServiceMock.PhoneExists.and.returnValue(of(false));
      userServiceMock.UserNameExists.and.returnValue(of(false));


      let form = component.createAccountForm;
      expect(form.valid).toBeFalsy();
      form.controls.email.setValue('email@email.co.uk');
      form.controls.password.setValue('Password1@');
      form.controls.firstName.setValue('joe');
      form.controls.lastName.setValue('blogs');
      form.controls.userName.setValue('joe.blogs');
      form.controls.phoneNumber.setValue('0000000000');
      form.controls.dOB.setValue(new Date(new Date().getFullYear() - 19, 0, 0));
      form.controls.remember.setValue('');


      fixture.detectChanges();
      tick();

      expect(form.valid).toBeTruthy();
    }));

    it('form should be invalid', fakeAsync(() =>
    {
      userServiceMock.EmailExists.and.returnValue(of(true));
      userServiceMock.PhoneExists.and.returnValue(of(true));
      userServiceMock.UserNameExists.and.returnValue(of(true));


      let form = component.createAccountForm;
      expect(form.valid).toBeFalsy();
      form.controls.email.setValue('email.email.co.uk');
      form.controls.password.setValue('bad password');
      form.controls.firstName.setValue(null);
      form.controls.lastName.setValue(null);
      form.controls.userName.setValue('joe.blogs');
      form.controls.phoneNumber.setValue('0000000000');
      form.controls.dOB.setValue(new Date(new Date().getFullYear() - 10, 0, 0));
      form.controls.remember.setValue('');


      fixture.detectChanges();
      tick();

      expect(form.invalid).toBeTruthy();
    }));
  });

  it('should create account', fakeAsync(() =>
  {
    let u: UserViewModel = {Avatar: "", Id: "", IsDarkMode: false, Token: "", UserName: ""};
    userServiceMock.CreateUser.and.returnValue(of(u));
    let routerSpy = spyOn(router, 'navigate');
    component.createAccount();

    fixture.detectChanges();
    tick();

    expect(component.loading).toBeFalsy();
    expect(routerSpy).toHaveBeenCalledWith(['/user/account']);
    expect(dialogRefMock.close).toHaveBeenCalled();
  }));

  it('should not create account', fakeAsync(() =>
  {
    let err: HttpErrorResponse = new HttpErrorResponse({
      error: "this is an error",
      headers: null,
      status: 401,
      statusText: null,
      url: null
    });
    userServiceMock.CreateUser.and.returnValue(throwError(err));
    component.createAccount();

    fixture.detectChanges();
    tick();

    expect(component.loading).toBeFalsy();
    expect(component.CreateError).toBe("this is an error");
  }));

  it('should file change event', () =>
  {
    let event: any = {target: {files:[{name: "image.png"}]}};
    component.fileChangeEvent(event);

    expect(component.avatarFileName).toBe("image.png");
    expect(component.imageChangedEvent).toBe(event);
  });

  it('should load image failed', () => {
    component.loadImageFailed();
    expect(component.avatarError).toBe("Failed to load image");
  });
});
