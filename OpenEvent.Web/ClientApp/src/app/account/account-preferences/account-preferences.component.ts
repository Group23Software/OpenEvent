import {Component, OnDestroy, OnInit} from '@angular/core';
import {ImageManipulationService} from "../../_extensions/image-manipulation.service";
import {HttpErrorResponse} from "@angular/common/http";
import {ConfirmDialogComponent} from "../../_extensions/confirm-dialog/confirm-dialog.component";
import {UserService} from "../../_Services/user.service";
import {UserValidatorsService} from "../../_Services/user-validators.service";
import {MatDialog} from "@angular/material/dialog";
import {Router} from "@angular/router";
import {AuthService} from "../../_Services/auth.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ImageUploadComponent, uploadConfig} from "../../_extensions/image-upload/image-upload.component";
import {TriggerService} from "../../_Services/trigger.service";
import {IteratorStatus} from "../../_extensions/iterator/iterator.component";

@Component({
  selector: 'account-preferences',
  templateUrl: './account-preferences.component.html',
  styleUrls: ['./account-preferences.component.css']
})
export class AccountPreferencesComponent implements OnInit, OnDestroy
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
  public newAddressError: string;

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

  public newAddressForm = new FormControl('', [Validators.required]);

  constructor (
    private userService: UserService,
    private userValidators: UserValidatorsService,
    private dialog: MatDialog,
    private router: Router,
    private authService: AuthService,
    private trigger: TriggerService
  )
  {
    this.userService.OpenConnection();
  }

  ngOnDestroy (): void
  {
    this.userService.DestroyConnection();
  }

  ngOnInit ()
  {
    this.username.setValue(this.user.UserName);
  }

  public UpdatePassword ()
  {
    this.updatePasswordLoading = true;
    this.authService.UpdatePassword({
      Id: this.user.Id,
      Password: this.newPasswordForm.value.password
    }).subscribe(response =>
    {
      this.updatePasswordLoading = false;
      this.trigger.Iterate('Updated password', 1000, IteratorStatus.good);
    }, (error: HttpErrorResponse) =>
    {
      this.updatePasswordLoading = false;
      this.newPasswordError = error.error.Message;
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
          this.deleteAccountError = error.error.Message
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
      this.trigger.Iterate('Updated username', 1000, IteratorStatus.good);
    }, (error: HttpErrorResponse) =>
    {
      this.updateUserNameLoading = false;
      this.newUserNameError = error.error.Message;
      this.userName.setErrors({http: true});
      console.error(error);
    });
  }

  public avatarUpload ()
  {
    let ref = this.dialog.open(ImageUploadComponent, {
      data: {
        height: 1,
        width: 1,
        isAvatar: true
      } as uploadConfig
    });

    ref.afterClosed().subscribe((image: string) =>
    {
      if (image)
      {
        this.updateAvatarLoading = true;
        this.userService.UpdateAvatar({
          Id: this.user.Id,
          Avatar: (new ImageManipulationService).toUTF8Array(image)
        }).subscribe(response =>
        {
          this.avatarFileName = null;
          this.trigger.Iterate('Updated avatar', 1000, IteratorStatus.good);
          this.updateAvatarLoading = false;
        }, (error: HttpErrorResponse) =>
        {
          this.newAvatarError = error.error.Message;
          this.updateAvatarLoading = false;
        });
      }

    });
  }

  public newAddress ()
  {
    this.userService.UpdateAddress({Id: this.user.Id, Address: this.newAddressForm.value}).subscribe(x =>
    {
      this.newAddressForm.disable();
      this.trigger.Iterate('Updated address', 1000, IteratorStatus.good);
    }, (e: HttpErrorResponse) =>
    {
      console.error(e);
      this.newAddressError = e.error.Message;
    });
  }
}
