import {Component, OnInit} from '@angular/core';
import {UserService} from "../_Services/user.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserValidatorsService} from "../_Services/user-validators.service";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../_extensions/confirm-dialog/confirm-dialog.component";
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "../_Services/auth.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ImageCroppedEvent} from "ngx-image-cropper";
import {ImageManipulationService} from "../_extensions/image-manipulation.service";


@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit
{
  public newPasswordError: string;
  public deleteAccountError: string;
  public newUserNameError: string;
  public newAvatarError: string;

  public imageChangedEvent: any = '';
  public croppedImage: any = '';


  get passwordConfirm ()
  {
    return this.newPasswordForm.get('passwordConfirm');
  }

  public newPasswordForm = new FormGroup({
    password: new FormControl('', [Validators.required]),
    passwordConfirm: new FormControl('', [Validators.required, this.userValidators.matches('password')])
  });
  public userName = new FormControl('', [Validators.required], [this.userValidators.usernameValidator()]);

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

  get username ()
  {
    return this.userName;
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
      this.snackBar.open('Updated password', 'close', {duration: 500})
    }, (error: HttpErrorResponse) =>
    {
      this.newPasswordError = error.message;
      this.newPasswordForm.setErrors({http: true});
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

  public UpdateUserName ()
  {
    this.userService.UpdateUserName({Id: this.user.Id, UserName: this.username.value}).subscribe(response =>
    {
      this.snackBar.open('Updated username', 'close', {duration: 500});
    }, (error: HttpErrorResponse) =>
    {
      this.newUserNameError = error.message;
      this.userName.setErrors({http: true});
      console.error(error);
    });
  }

  public fileChangeEvent (event: any): void
  {
    this.imageChangedEvent = event;
  }

  public imageCropped (event: ImageCroppedEvent): void
  {
    this.croppedImage = event.base64;
  }

  public loadImageFailed () : void
  {
    // TODO: error message
  }

  public NewAvatar ()
  {
    this.userService.UpdateAvatar({Id: this.user.Id, Avatar: (new ImageManipulationService).toUTF8Array(this.croppedImage)}).subscribe(response =>
    {
      this.snackBar.open('Updated avatar', 'close', {duration: 500});
    }, (error: HttpErrorResponse) =>
    {
      this.newAvatarError = error.message;
      console.error(error);
    });
  }
}
