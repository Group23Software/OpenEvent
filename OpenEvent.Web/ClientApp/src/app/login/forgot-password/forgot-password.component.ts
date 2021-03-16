import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserValidatorsService} from "../../_Services/user-validators.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../_Services/auth.service";
import {IteratorStatus} from "../../_extensions/iterator/iterator.component";
import {HttpErrorResponse} from "@angular/common/http";
import {TriggerService} from "../../_Services/trigger.service";

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit
{
  public newPasswordForm = new FormGroup({
    password: new FormControl('', [Validators.required]),
    passwordConfirm: new FormControl('', [Validators.required, this.userValidators.matches('password')])
  });
  public newPasswordError: string;
  public loading: boolean;

  private id: string;

  get passwordConfirm ()
  {
    return this.newPasswordForm.get('passwordConfirm');
  }

  constructor (private userValidators: UserValidatorsService, private authService: AuthService, private route: ActivatedRoute, private trigger: TriggerService, private router: Router)
  {
  }

  ngOnInit (): void
  {
    this.id = this.route.snapshot.paramMap.get('id');
  }

  public resetPassword (): void
  {
    this.authService.UpdatePassword({
      Password: this.newPasswordForm.controls.password.value,
      Id: this.id
    }).subscribe(response =>
    {
      this.trigger.Iterate('Updated password',1000,IteratorStatus.good);
      this.router.navigate(['/login']);
    }, (error: HttpErrorResponse) =>
    {
      this.newPasswordError = error.error.Message;
      this.newPasswordForm.setErrors({http: true});
      console.error(error);
    }, () => this.loading = false);
  }
}
