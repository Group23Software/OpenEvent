import {Component, OnInit} from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {AuthService} from "../../_Services/auth.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-forgot-password-dialog',
  templateUrl: './forgot-password-dialog.component.html',
  styleUrls: ['./forgot-password-dialog.component.css']
})
export class ForgotPasswordDialogComponent implements OnInit
{
  public email: FormControl = new FormControl('', [Validators.required, Validators.email]);
  public loading: boolean = false;
  public success: boolean = false;
  public error: string;

  constructor (private authService: AuthService)
  {
  }

  ngOnInit (): void
  {
  }

  forgetPassword ()
  {
    this.loading = true;
    this.authService.ForgetPassword(this.email.value).subscribe(
      () => this.success = true,
      (e: HttpErrorResponse) => this.error = e.error.Message,
      () => this.loading = false);
  }
}
