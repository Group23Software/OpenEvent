import {async, ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';

import {LoginComponent} from './login.component';
import {PublicNavComponent} from "../navs/public-nav/public-nav.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {LoadingComponent} from "../loading/loading.component";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatButtonModule} from "@angular/material/button";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {BrowserDynamicTestingModule} from "@angular/platform-browser-dynamic/testing";
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {CookieService} from "ngx-cookie-service";
import {RouterTestingModule} from "@angular/router/testing";
import {MatDialog, MatDialogModule} from "@angular/material/dialog";
import {MatInputModule} from "@angular/material/input";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {Router} from "@angular/router";
import {Observable, of, throwError} from "rxjs";
import {AuthService} from "../_Services/auth.service";
import {UserViewModel} from "../_models/User";
import {HttpErrorResponse} from "@angular/common/http";
// import 'jest';

describe('LoginComponent', () =>
{
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let dialogMock;
  let authMock;
  let router;

  beforeEach(async(() =>
  {
    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    authMock = jasmine.createSpyObj('authService', ['IsAuthenticated', 'Login']);
    authMock.IsAuthenticated.and.returnValue(of(false));

    // routerMock = jasmine.createSpyObj('router', ['navigate','routerLink','routerLinkActive']);
    // routerMock.navigate.and.returnValue(null);

    TestBed.configureTestingModule({
      declarations: [
        LoginComponent,
        PublicNavComponent,
        LoadingComponent
      ], imports: [
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatCheckboxModule,
        MatToolbarModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        BrowserDynamicTestingModule,
        HttpClientTestingModule,
        MatDialogModule,
        MatInputModule,
        BrowserAnimationsModule,
        RouterTestingModule,
      ],
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: MatDialog, useValue: dialogMock},
        {provide: AuthService, useValue: authMock},
        CookieService
      ]
    }).compileComponents();
  }));

  beforeEach(() =>
    {
      router = TestBed.inject(Router);
      fixture = TestBed.createComponent(LoginComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }
  );

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should redirect to app', fakeAsync(() =>
  {
    authMock.IsAuthenticated.and.returnValue(of(true));
    const routerSpy = spyOn(router, 'navigate');

    component.ngOnInit();
    tick();

    expect(authMock.IsAuthenticated).toHaveBeenCalled();
    expect(routerSpy).toHaveBeenCalledWith(['/user/account']);

  }));

  it('should login', fakeAsync(() =>
  {
    let u: UserViewModel = {Avatar: "", Id: "", IsDarkMode: false, Token: "", UserName: ""}
    authMock.Login.and.returnValue(of(u));
    const routerSpy = spyOn(router, 'navigate');

    component.loginFormGroup.controls.email.setValue('email@email.co.uk');
    component.loginFormGroup.controls.password.setValue('Password');
    expect(component.loginFormGroup.valid).toBeTruthy();

    component.login();

    expect(routerSpy).toHaveBeenCalledWith(['/user/account']);

  }));

  it('should not login', fakeAsync(() =>
  {
    let err: HttpErrorResponse = new HttpErrorResponse({
      error: "this is an error",
      headers: null,
      status: 401,
      statusText: null,
      url: null
    });
    authMock.Login.and.returnValue(throwError(err));

    component.loginFormGroup.controls.email.setValue('email@email.co.uk');
    component.loginFormGroup.controls.password.setValue('Password');
    expect(component.loginFormGroup.valid).toBeTruthy();

    component.login();

    expect(component.loading).toBeFalsy();
    expect(component.loginError).toBe("this is an error");

  }));

  it('should declare instance variables', async(() =>
  {
    expect(component.loading).toEqual(false);
    expect(component.loginError).toBeNull();
  }));

  it('should render input elements', async(() =>
  {
    const compiled = fixture.debugElement.nativeElement;
    const emailInput = compiled.querySelector('input[id="email"]');
    const passwordInput = compiled.querySelector('input[id="password"]');

    expect(emailInput).toBeTruthy();
    expect(passwordInput).toBeTruthy();
  }));

  it('should open create account dialog', () =>
  {
    component.create();
    expect(dialogMock.open).toHaveBeenCalled();
  });

  describe('form validation', () =>
  {

    it('email should be valid', () =>
    {
      let form = component.loginFormGroup;
      form.controls.email.setValue('email@google.com');
      expect(form.controls.email.valid).toBeTruthy();
    });

    it('email should be invalid', () =>
    {
      let form = component.loginFormGroup;
      form.controls.email.setValue('email&google.com.co');
      expect(form.controls.email.invalid).toBeTruthy();
    });

    it('form should be valid', () =>
    {
      let form = component.loginFormGroup;
      expect(form.valid).toBeFalsy();
      form.controls.email.setValue('email@google.com');
      form.controls.password.setValue('password');
      expect(form.valid).toBeTruthy();
    });

    it('form should be invalid', () =>
    {
      let form = component.loginFormGroup;
      form.controls.email.setValue('email&google.com.co');
      form.controls.password.setValue('password');
      expect(form.invalid).toBeTruthy();
    });

    it('should test email input errors', () =>
    {
      let emailInput = component.loginFormGroup.controls.email;
      expect(emailInput.errors.required).toBeTruthy();
      emailInput.setValue('email&google.com.co');
      expect(emailInput.errors.email).toBeTruthy();
      emailInput.setValue('email@google.com');
      expect(emailInput.errors).toBeNull();
    });

    it('should test password input errors', () =>
    {
      let passwordInput = component.loginFormGroup.controls.password;
      expect(passwordInput.errors.required).toBeTruthy();
      passwordInput.setValue('password');
      expect(passwordInput.errors).toBeNull();
    });
  });
});
