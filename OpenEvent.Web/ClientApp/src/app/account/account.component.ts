import {Component, OnInit} from '@angular/core';
import {UserService} from "../_Services/user.service";
import {AbstractControl, FormControl, FormGroup, Validators} from "@angular/forms";
import {UserValidatorsService} from "../_Services/user-validators.service";
import {MatDividerModule} from "@angular/material/divider";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDialogComponent, ConfirmDialogData} from "../_extensions/confirm-dialog/confirm-dialog.component";
import {HttpErrorResponse, HttpResponse} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "../_Services/auth.service";
import {MatSnackBar} from "@angular/material/snack-bar";


@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit
{
  public newPasswordError: string;
  public deleteAccountError: string;

  get passwordConfirm ()
  {
    return this.newPasswordForm.get('passwordConfirm');
  }

  public newPasswordForm = new FormGroup({
    password: new FormControl('', [Validators.required]),
    passwordConfirm: new FormControl('', [Validators.required,this.userValidators.matches('password')])
  });

  constructor (
    private userService: UserService,
    private userValidators: UserValidatorsService,
    private dialog: MatDialog,
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar)
  {
  }

  ngOnInit ()
  {
  }

  get user ()
  {
    return this.userService.User;
  }

  public UpdatePassword ()
  {
    this.authService.UpdatePassword({
      Email: this.user.Email,
      Password: this.newPasswordForm.value.password
    }).subscribe(response =>
    {
      this.snackBar.open('Updated password','close',{duration: 500})
    }, (error: HttpErrorResponse) =>
    {
      this.newPasswordError = error.message;
      console.error(error);
    })
  }

  public DeleteAccount ()
  {
    let deleteAccount = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: 'Are you sure you want to permanently delete your account, this cannot be undone',
        color: 'warn'
      }
    });

    deleteAccount.afterClosed().subscribe(result =>
    {
      if (result)
      {
        this.userService.Destroy(this.user.Id).subscribe((response) =>
        {
          this.router.navigate(['/login']);
        }, (error: HttpErrorResponse) =>
        {
          console.error(error);
          this.deleteAccountError = error.message
        });
      }
    });
  }
}
