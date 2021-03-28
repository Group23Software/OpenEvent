import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ForgotPasswordDialogComponent} from './forgot-password-dialog.component';
import {AuthService} from "../../_Services/auth.service";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

describe('ForgotPasswordDialogComponent', () =>
{
  let component: ForgotPasswordDialogComponent;
  let fixture: ComponentFixture<ForgotPasswordDialogComponent>;

  let authServiceMock;

  beforeEach(async () =>
  {

    authServiceMock = jasmine.createSpyObj('AuthService', ['ForgetPassword']);

    await TestBed.configureTestingModule({
      imports: [
        MatFormFieldModule,
        MatInputModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule
      ],
      declarations: [ForgotPasswordDialogComponent],
      providers: [
        {provide: AuthService, useValue: authServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(ForgotPasswordDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
