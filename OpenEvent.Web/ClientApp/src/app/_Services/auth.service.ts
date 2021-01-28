import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpResponse} from "@angular/common/http";
import {Observable, of, throwError} from "rxjs";
import {AuthBody, UpdatePasswordBody, UserViewModel} from "../_Models/User";
import {catchError, flatMap, map, switchMap} from "rxjs/operators";
import {CookieService} from "ngx-cookie-service";
import jwtDecode, {JwtPayload} from "jwt-decode";
import {UserService} from "./user.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService
{

  private Token: string;
  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private cookieService: CookieService, private userService: UserService)
  {
    this.BaseUrl = baseUrl;
    this.BaseUrl = 'http://localhost:5000/';
  }

  public Login (auth: AuthBody): Observable<UserViewModel>
  {
    return this.http.post<UserViewModel>(this.BaseUrl + 'api/auth/authenticate', auth).pipe(
      map(user =>
      {
        console.log(user);
        this.Token = user.Token;
        let payload: JwtPayload = jwtDecode(this.Token);
        this.cookieService.set('token', this.Token, new Date(payload.exp * 1000));
        this.cookieService.set('id', user.Id, new Date(payload.exp * 1000));
        this.userService.User = user;
        return user;
      })
    );
  }

  public Authorise (id: string): Observable<UserViewModel>
  {
    return this.http.post<UserViewModel>(this.BaseUrl + 'api/auth/authenticateToken', {id: id}).pipe(map(user =>
    {
      this.userService.User = user;
      // this.userService.User.Avatar = 'data:image/png;base64,' + this.userService.User.Avatar;
      console.log(this.userService.User)
      return user;
    }));
  }

  public IsAuthenticated (): Observable<boolean>
  {
    return this.userService.GetUserAsync().pipe(
      switchMap(u =>
        {
          if (u == null)
          {
            console.log("There is no user");
            let id = this.cookieService.get('id');
            if (this.cookieService.check('id'))
            {
              console.log("Client has token saved, getting user");
              return this.Authorise(id).pipe(map(x => !!x));
            }
            return of(false);
          }
          return of(true);
        }
      ))
  }

  public GetToken (): string
  {
    return this.cookieService.get('token');
  }

  public UpdatePassword (updatePasswordBody: UpdatePasswordBody): Observable<any>
  {
    return this.http.post(this.BaseUrl + 'api/auth/updatePassword', updatePasswordBody);
  }
}
