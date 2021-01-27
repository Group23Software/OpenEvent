import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {min} from "rxjs/operators";
import {AuthService} from "../_Services/auth.service";
import {HttpErrorResponse, HttpResponse} from "@angular/common/http";
import {Router} from "@angular/router";

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
    password: new FormControl('', [Validators.required, Validators.min(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$')]),
    remember: new FormControl('')
  });
  loading: boolean = false;
  loginError: string = null;


  constructor (private authService: AuthService, private router: Router)
  {
  }

  get email ()
  {
    return this.loginFormGroup.get('email')
  }

  ngOnInit ()
  {
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
        this.router.navigate(['/account']).then(r => {
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

  }
}
