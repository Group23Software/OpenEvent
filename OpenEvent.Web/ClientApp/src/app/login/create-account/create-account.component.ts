import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserValidatorsService} from "../../_Services/user-validators.service";
import {ImageCroppedEvent} from "ngx-image-cropper";
import {UserService} from "../../_Services/user.service";
import {NewUserInput} from "../../_Models/User";
import {MatDialogRef} from "@angular/material/dialog";
import {Router} from "@angular/router";
import {HttpErrorResponse} from "@angular/common/http";
import {DefaultProfile} from "./default-profile";
import {ImageManipulationService} from "../../_extensions/image-manipulation.service";

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.css']
})
export class CreateAccountComponent implements OnInit
{

  public DefaultProfile = DefaultProfile;
  public CreateError: string;
  public maxDate: Date;

  public createAccountForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email], [this.userValidators.emailValidator()]),
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    userName: new FormControl('', [Validators.required], [this.userValidators.usernameValidator()]),
    password: new FormControl('', [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$')]),
    phoneNumber: new FormControl('', [Validators.required], [this.userValidators.phoneValidator()]),
    dOB: new FormControl('', [Validators.required]),
    remember: new FormControl('')
  });

  public imageChangedEvent: any = '';
  public croppedImage: any = '';
  public loading: boolean = false;

  public get email ()
  {
    return this.createAccountForm.get('email');
  }

  public get username ()
  {
    return this.createAccountForm.get('userName');
  }

  public get phoneNumber ()
  {
    return this.createAccountForm.get('phoneNumber');
  }

// , Validators.pattern('^(((\\+44\\s?\\d{4}|\\(?0\\d{4}\\)?)\\s?\\d{3}\\s?\\d{3})|((\\+44\\s?\\d{3}|\\(?0\\d{3}\\)?)\\s?\\d{3}\\s?\\d{4})|((\\+44\\s?\\d{2}|\\(?0\\d{2}\\)?)\\s?\\d{4}\\s?\\d{4}))(\\s?\\#(\\d{4}|\\d{3}))?$\n')

  constructor (private userValidators: UserValidatorsService, private userService: UserService, private dialogRef: MatDialogRef<CreateAccountComponent>, private router: Router)
  {
    const currentYear = new Date().getFullYear();
    this.maxDate = new Date(currentYear - 18, 0, 0);
  }

  ngOnInit ()
  {
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

  public createAccount () : void
  {
    this.loading = true;

    let newUserInput: NewUserInput = {
      Email: this.createAccountForm.value.email,
      FirstName: this.createAccountForm.value.firstName,
      LastName: this.createAccountForm.value.lastName,
      UserName: this.createAccountForm.value.userName,
      DateOfBirth: this.createAccountForm.value.dOB,
      PhoneNumber: this.createAccountForm.value.phoneNumber,
      Avatar: (new ImageManipulationService).toUTF8Array(this.croppedImage),
      Password: this.createAccountForm.value.password,
      Remember: this.createAccountForm.value.remember == true
    }

    this.userService.CreateUser(newUserInput).subscribe((response) =>
    {
      // TODO: redirect to app
      this.loading = false;
      this.router.navigate(['/account']);
      this.dialogRef.close();
    }, (error: HttpErrorResponse) =>
    {
      console.error(error);
      this.loading = false;
      this.CreateError = error.error
    })
  }
}
