import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../_Services/auth.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {CreateAccountComponent} from "./create-account/create-account.component";
import {ForgotPasswordDialogComponent} from "./forgot-password-dialog/forgot-password-dialog.component";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit
{
  public loginFormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    remember: new FormControl('')
  });
  public loading: boolean = false;
  public loginError: string = null;


  constructor (private authService: AuthService, private router: Router, private dialog: MatDialog)
  {
  }

  get email ()
  {
    return this.loginFormGroup.get('email')
  }

  ngOnInit ()
  {
    // TODO: this might be breaking stuff
    this.authService.IsAuthenticated().subscribe(isAuthenticated => {
      if (isAuthenticated) this.router.navigate(['/explore']);
    });
  }

  public login ()
  {
    console.log("login has been pressed", this.loginFormGroup);
    this.loading = true;

    this.authService.Login({
      Email: this.loginFormGroup.value.email,
      Password: this.loginFormGroup.value.password,
      Remember: this.loginFormGroup.value.remember == true
    }).subscribe(
      (response) =>
      {
        this.router.navigate(['/explore']);
      },
      (error: HttpErrorResponse) =>
      {
        console.error(error);
        this.loading = false;
        this.loginError = error.error;
      });
  }

  public create (): void
  {
    let dialog = this.dialog.open(CreateAccountComponent, {
      width: "80vw"
    });
  }

  public openForgotDialog (): void
  {
    let dialog = this.dialog.open(ForgotPasswordDialogComponent, {
      width: '60vh'
    });
  }
}
