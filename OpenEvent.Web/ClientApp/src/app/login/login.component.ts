import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../_Services/auth.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {CreateAccountComponent} from "./create-account/create-account.component";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit
{

  hide = true;

  loginFormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    remember: new FormControl('')
  });
  loading: boolean = false;
  loginError: string = null;


  constructor (private authService: AuthService, private router: Router, private dialog: MatDialog)
  {
  }

  get email ()
  {
    return this.loginFormGroup.get('email')
  }

  ngOnInit ()
  {
    this.authService.IsAuthenticated().subscribe(isAuthenticated => {
      // TODO : This will need to navigate to app not account in future
      if (isAuthenticated) this.router.navigate(['/account']);
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
        // TODO: redirect to app
        this.router.navigate(['/account']).then(r =>
        {
          if (r) console.log('Navigated');
        });
      },
      (error: HttpErrorResponse) =>
      {
        console.error(error);
        this.loading = false;
        this.loginError = error.error;
      });
  }

  public create ()
  {
    console.log("sign up has been pressed");
    let dialog = this.dialog.open(CreateAccountComponent, {
      width: "80vw"
    });
  }
}
