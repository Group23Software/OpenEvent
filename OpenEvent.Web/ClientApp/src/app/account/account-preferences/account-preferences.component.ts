import {Component, OnInit} from '@angular/core';
import {ImageManipulationService} from "../../_extensions/image-manipulation.service";
import {HttpErrorResponse} from "@angular/common/http";
import {ConfirmDialogComponent} from "../../_extensions/confirm-dialog/confirm-dialog.component";
import {ImageCroppedEvent} from "ngx-image-cropper";
import {UserService} from "../../_Services/user.service";
import {UserValidatorsService} from "../../_Services/user-validators.service";
import {MatDialog} from "@angular/material/dialog";
import {Router} from "@angular/router";
import {AuthService} from "../../_Services/auth.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'account-preferences',
  templateUrl: './account-preferences.component.html',
  styleUrls: ['./account-preferences.component.css']
})
export class AccountPreferencesComponent implements OnInit
{

  get username ()
  {
    return this.userName;
  }

  get user ()
  {
    return this.userService.User;
  }

  public newPasswordError: string;
  public deleteAccountError: string;
  public newUserNameError: string;
  public newAvatarError: string;

  public imageChangedEvent: any = '';
  public croppedImage: any = '';
  public avatarFileName: string;
  public updatePasswordLoading: boolean = false;
  public updateUserNameLoading: boolean = false;
  public updateAvatarLoading: boolean = false;


  get passwordConfirm ()
  {
    return this.newPasswordForm.get('passwordConfirm');
  }

  public newPasswordForm = new FormGroup({
    password: new FormControl('', [Validators.required]),
    passwordConfirm: new FormControl('', [Validators.required, this.userValidators.matches('password')])
  });
  public userName = new FormControl('', [Validators.required], [this.userValidators.usernameValidator()]);

  constructor (private userService: UserService,
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

  public UpdatePassword ()
  {
    this.updatePasswordLoading = true;
    this.authService.UpdatePassword({
      Email: this.user.Email,
      Password: this.newPasswordForm.value.password
    }).subscribe(response =>
    {
      this.updatePasswordLoading = false;
      this.snackBar.open('Updated password', 'close', {duration: 500})
    }, (error: HttpErrorResponse) =>
    {
      this.updatePasswordLoading = false;
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
    this.updateUserNameLoading = true;
    this.userService.UpdateUserName({Id: this.user.Id, UserName: this.username.value}).subscribe(response =>
    {
      this.updateUserNameLoading = false;
      this.snackBar.open('Updated username', 'close', {duration: 500});
    }, (error: HttpErrorResponse) =>
    {
      this.updateUserNameLoading = false;
      this.newUserNameError = error.message;
      this.userName.setErrors({http: true});
      console.error(error);
    });
  }

  public fileChangeEvent (event: any): void
  {
    this.avatarFileName = event.target.files[0].name;
    this.imageChangedEvent = event;
    this.newAvatarError = null;
  }

  public imageCropped (event: ImageCroppedEvent): void
  {
    this.croppedImage = event.base64;
  }

  public loadImageFailed (): void
  {
    this.newAvatarError = "Failed to load image";
  }

  public UpdateAvatar ()
  {
    this.updateAvatarLoading = true;
    this.userService.UpdateAvatar({
      Id: this.user.Id,
      Avatar: (new ImageManipulationService).toUTF8Array(this.croppedImage)
    }).subscribe(response =>
    {
      this.updateAvatarLoading = false;
      this.avatarFileName = null;
      this.snackBar.open('Updated avatar', 'close', {duration: 500});
    }, (error: HttpErrorResponse) =>
    {
      this.updateAvatarLoading = false;
      this.newAvatarError = error.message;
      console.error(error);
    });
  }
}