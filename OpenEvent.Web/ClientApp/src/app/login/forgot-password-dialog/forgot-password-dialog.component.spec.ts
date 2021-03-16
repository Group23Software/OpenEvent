import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ForgotPasswordDialogComponent} from './forgot-password-dialog.component';
import {AuthService} from "../../_Services/auth.service";

describe('ForgotPasswordDialogComponent', () =>
{
  let component: ForgotPasswordDialogComponent;
  let fixture: ComponentFixture<ForgotPasswordDialogComponent>;

  let authServiceMock;

  beforeEach(async () =>
  {

    authServiceMock = jasmine.createSpyObj('AuthService',['ForgetPassword']);

    await TestBed.configureTestingModule({
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
