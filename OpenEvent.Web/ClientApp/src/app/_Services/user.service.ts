import {Inject, Injectable} from '@angular/core';
import {NewUserInput, UserViewModel} from "../_Models/User";
import {Observable, of} from "rxjs";
import {HttpClient, HttpParams} from "@angular/common/http";
import {CookieService} from "ngx-cookie-service";
import {map} from "rxjs/operators";
import jwtDecode, {JwtPayload} from "jwt-decode";

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

  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private cookieService: CookieService)
  {
    this.BaseUrl = baseUrl;
    this.BaseUrl = 'http://localhost:5000/';
  }

  public GetUserAsync (): Observable<UserViewModel | null>
  {
    // console.log(this._User);
    return of(this._User);
  }

  public CreateUser (newUserInput: NewUserInput): Observable<UserViewModel>
  {
    return this.http.post<UserViewModel>(this.BaseUrl + 'api/user', newUserInput).pipe(
      map(user =>
      {
        console.log(user);
        // this.Token = user.Token;
        let payload: JwtPayload = jwtDecode(user.Token);
        this.cookieService.set('token', user.Token, new Date(payload.exp * 1000));
        this.cookieService.set('id', user.Id, new Date(payload.exp * 1000));
        this.User = user;
        return user;
      })
    );
  }

  public UserNameExists (username: string): Observable<boolean>
  {
    return this.http.get<boolean>(this.BaseUrl + 'api/user/UserNameExists', {params: new HttpParams().set("username", username)});
  }

  public EmailExists (email: string): Observable<boolean>
  {
    return this.http.get<boolean>(this.BaseUrl + 'api/user/EmailExists', {params: new HttpParams().set("email", email)});
  }

  public PhoneExists (phoneNumber: string): Observable<boolean>
  {
    return this.http.get<boolean>(this.BaseUrl + 'api/user/PhoneExists', {params: new HttpParams().set("phoneNumber", phoneNumber)});
  }
}
