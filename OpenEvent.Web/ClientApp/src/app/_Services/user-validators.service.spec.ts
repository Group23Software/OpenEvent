import {TestBed} from "@angular/core/testing";
import {UserValidatorsService} from "./user-validators.service";
import {FormControl, FormGroup} from "@angular/forms";
import {UserService} from "./user.service";
import {of} from "rxjs";

describe('UserValidatorsService', () =>
{
  let userServiceMock;

  beforeEach(() =>
  {

    userServiceMock = jasmine.createSpyObj('userService', ['EmailExists','PhoneExists','UserNameExists']);

    TestBed.configureTestingModule({
      providers: [
        {provide: UserService, useValue: userServiceMock},
        UserValidatorsService
      ]
    });
  });

  it('FormControls should match',  () =>
  {
    const service: UserValidatorsService = TestBed.inject(UserValidatorsService);
    let formGroup: FormGroup = new FormGroup({
      email: new FormControl(''),
      emailConfirm: new FormControl('',[service.matches('email')])
    });
    expect(formGroup.errors).toBeNull();
    let email = 'email@email.co.uk';
    formGroup.controls.email.setValue(email);
    formGroup.controls.emailConfirm.setValue(email);
    expect(formGroup.controls.emailConfirm.errors).toBeNull();
  });

  it('FormControls should not match',  () =>
  {
    const service: UserValidatorsService = TestBed.inject(UserValidatorsService);
    let formGroup: FormGroup = new FormGroup({
      email: new FormControl(''),
      emailConfirm: new FormControl('',[service.matches('email')])
    });
    expect(formGroup.errors).toBeNull();
    formGroup.controls.email.setValue('email@email.co.uk');
    formGroup.controls.emailConfirm.setValue('different@email.co.uk');
    expect(formGroup.controls.emailConfirm.hasError('matches')).toBeTruthy();
  });

  it('should validate username', () =>
  {
    userServiceMock.UserNameExists.and.returnValue(of(false));
    const service: UserValidatorsService = TestBed.inject(UserValidatorsService);
    let formControl: FormControl = new FormControl('');
    formControl.setAsyncValidators(service.usernameValidator());
    expect(formControl.errors).toBeNull();
    formControl.setValue('username');
    expect(formControl.errors).toBeNull();
  });

  it('should invalidate username', () =>
  {
    userServiceMock.UserNameExists.and.returnValue(of(true));
    const service: UserValidatorsService = TestBed.inject(UserValidatorsService);
    let formControl: FormControl = new FormControl('');
    formControl.setAsyncValidators(service.usernameValidator());
    expect(formControl.errors).toBeNull();
    formControl.setValue('username');
    expect(formControl.errors.usernameExists).toBeTruthy();
  });

  it('should validate email', () =>
  {
    userServiceMock.EmailExists.and.returnValue(of(false));
    const service: UserValidatorsService = TestBed.inject(UserValidatorsService);
    let formControl: FormControl = new FormControl('');
    formControl.setAsyncValidators(service.emailValidator());
    expect(formControl.errors).toBeNull();
    formControl.setValue('email@email.co.uk');
    expect(formControl.errors).toBeNull();
  });

  it('should invalidate email', () =>
  {
    userServiceMock.EmailExists.and.returnValue(of(true));
    const service: UserValidatorsService = TestBed.inject(UserValidatorsService);
    let formControl: FormControl = new FormControl('');
    formControl.setAsyncValidators(service.emailValidator());
    expect(formControl.errors).toBeNull();
    formControl.setValue('email@email.co.uk');
    expect(formControl.errors.emailExists).toBeTruthy();
  });

  it('should validate phone number', () =>
  {
    userServiceMock.PhoneExists.and.returnValue(of(false));
    const service: UserValidatorsService = TestBed.inject(UserValidatorsService);
    let formControl: FormControl = new FormControl('');
    formControl.setAsyncValidators(service.phoneValidator());
    expect(formControl.errors).toBeNull();
    formControl.setValue('0000000000');
    expect(formControl.errors).toBeNull();
  });

  it('should invalidate phone number', () =>
  {
    userServiceMock.PhoneExists.and.returnValue(of(true));
    const service: UserValidatorsService = TestBed.inject(UserValidatorsService);
    let formControl: FormControl = new FormControl('');
    formControl.setAsyncValidators(service.phoneValidator());
    expect(formControl.errors).toBeNull();
    formControl.setValue('0000000000');
    expect(formControl.errors.phoneExists).toBeTruthy();
  });
});
