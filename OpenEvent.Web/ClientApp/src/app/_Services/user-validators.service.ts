import {Injectable} from '@angular/core';
import {AbstractControl, AsyncValidatorFn, FormControl, FormGroup, ValidationErrors, ValidatorFn} from "@angular/forms";
import {Observable, timer} from "rxjs";
import {UserService} from "./user.service";
import {debounceTime, map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class UserValidatorsService
{

  constructor (private userService: UserService)
  {
  }

  public matches(matchTo: string): (AbstractControl) => ValidationErrors | null {
    return (control: AbstractControl): ValidationErrors | null => {
      return !!control.parent &&
      !!control.parent.value &&
      control.value === control.parent.controls[matchTo].value
        ? null
        : { matches: true };
    };
  }

  // public matches (s, d): ValidatorFn
  // {
  //   return (group: FormGroup): { [key: string]: any } =>
  //   {
  //     let sC = group.get(s).value;
  //     let dC = group.get(d).value;
  //
  //     console.log(sC, dC);
  //
  //     return sC === dC ? null : {matches: true};
  //   }
  // }

  public usernameValidator (): AsyncValidatorFn
  {
    return (control: AbstractControl): Observable<ValidationErrors | null> =>
    {
      return this.userService.UserNameExists(control.value).pipe(
        map(res => res ? {usernameExists: true} : null)
      );
    };
  }

  public emailValidator (): AsyncValidatorFn
  {
    return (control: AbstractControl): Observable<ValidationErrors | null> =>
    {
      return this.userService.EmailExists(control.value).pipe(
        debounceTime(500),
        map(res => res ? {emailExists: true} : null)
      );
    };
  }

  public phoneValidator (): AsyncValidatorFn
  {
    return (control: AbstractControl): Observable<ValidationErrors | null> =>
    {
      return this.userService.PhoneExists(control.value).pipe(
        debounceTime(500),
        map(res => res ? {phoneExists: true} : null)
      );
    };
  }
}
