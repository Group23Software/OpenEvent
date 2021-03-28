import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ForgotPasswordComponent} from './forgot-password.component';
import {TriggerService} from "../../_Services/trigger.service";
import {UserValidatorsService} from "../../_Services/user-validators.service";
import {AuthService} from "../../_Services/auth.service";
import {ActivatedRoute, convertToParamMap} from "@angular/router";
import {RouterTestingModule} from "@angular/router/testing";
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatCardModule} from "@angular/material/card";
import {MatIconModule} from "@angular/material/icon";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

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
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        MatCardModule,
        MatIconModule,
        MatFormFieldModule,
        MatInputModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule
      ],
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
