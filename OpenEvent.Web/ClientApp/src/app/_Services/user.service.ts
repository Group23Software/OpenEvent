import {Injectable} from '@angular/core';
import {UserViewModel} from "../_Models/User";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService
{
  set User (value: UserViewModel)
  {
    this._User = value;
  }

  get User (): UserViewModel
  {
    return this._User;
  }

  private _User: UserViewModel;

  constructor ()
  {
  }

  public GetUserAsync() : Observable<UserViewModel | null> {
    // console.log(this._User);
    return of(this._User);
  }
}
