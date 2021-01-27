import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {min} from "rxjs/operators";

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
    password: new FormControl('', [Validators.required, Validators.min(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$')])
  });
  loading: boolean = false;


  constructor ()
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
    console.log("login has been pressed");
    this.loading = true;

  }

  public create ()
  {
    console.log("sign up has been pressed");

  }
}
