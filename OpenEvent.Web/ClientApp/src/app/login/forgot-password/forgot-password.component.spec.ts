import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ForgotPasswordComponent} from './forgot-password.component';
import {TriggerService} from "../../_Services/trigger.service";
import {UserValidatorsService} from "../../_Services/user-validators.service";
import {AuthService} from "../../_Services/auth.service";
import {ActivatedRoute, convertToParamMap} from "@angular/router";
import {RouterTestingModule} from "@angular/router/testing";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('ForgotPasswordComponent', () =>
{
  let component: ForgotPasswordComponent;
  let fixture: ComponentFixture<ForgotPasswordComponent>;

  let authServiceMock;
  let triggerServiceMock;
  // let userValidatorsServiceMock;

  beforeEach(async () =>
  {

    // userValidatorsServiceMock = jasmine.createSpyObj('UserValidatorsService', ['matches']);

    authServiceMock = jasmine.createSpyObj('AuthService', ['UpdatePassword']);
    triggerServiceMock = jasmine.createSpyObj('TriggerService', ['Iterate']);

    await TestBed.configureTestingModule({
      declarations: [ForgotPasswordComponent],
      imports: [RouterTestingModule, HttpClientTestingModule],
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: AuthService, useValue: authServiceMock},
        {provide: TriggerService, useValue: triggerServiceMock},
        UserValidatorsService,
        {
          provide: ActivatedRoute, useValue: {
            snapshot: {
              paramMap: convertToParamMap({id: "1"})
            }
          }
        }
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(ForgotPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
