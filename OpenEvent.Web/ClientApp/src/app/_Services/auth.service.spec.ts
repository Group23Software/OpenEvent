import {async, TestBed} from '@angular/core/testing';

import {AuthService} from './auth.service';
import {CookieService} from "ngx-cookie-service";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {UserService} from "./user.service";
import {of} from "rxjs";
import {UserViewModel} from "../_models/User";

describe('AuthService', () =>
{
  let httpClientMock;
  let userServiceMock;
  let cookieServiceMock;

  beforeEach(() =>
    {
      userServiceMock = jasmine.createSpyObj('userService', ['GetUserAsync']);
      userServiceMock.GetUserAsync.and.returnValue(of({}));

      cookieServiceMock = jasmine.createSpyObj('cookieService', ['get', 'set', 'check']);
      cookieServiceMock.get.and.returnValue('');
      cookieServiceMock.set.and.callThrough();
      cookieServiceMock.check.and.callThrough();

      httpClientMock = jasmine.createSpyObj('httpClient', ['post']);
      httpClientMock.post.and.returnValue(jasmine.createSpyObj("post", ["subscribe"]));

      TestBed.configureTestingModule({
        imports: [],
        providers: [
          {provide: 'BASE_URL', useValue: ''},
          {provide: HttpClient, useValue: httpClientMock},
          {provide: UserService, useValue: userServiceMock},
          {provide: CookieService, useValue: cookieServiceMock},
          AuthService
        ]
      });
    }
  )
  ;

  it('should be created', () =>
  {
    const service: AuthService = TestBed.inject(AuthService);
    expect(service).toBeTruthy();
  });

  it('should be authenticated', () =>
  {
    let u: UserViewModel = {Avatar: "", Id: "", IsDarkMode: false, Token: "", UserName: ""};
    userServiceMock.GetUserAsync.and.returnValue(of(u));
    const service: AuthService = TestBed.inject(AuthService);
    service.IsAuthenticated().subscribe(result => expect(result).toBe(true));
  });

  it('should be authenticated using token', () =>
  {
    let u: UserViewModel = {Avatar: "", Id: "", IsDarkMode: false, Token: "", UserName: ""};
    userServiceMock.GetUserAsync.and.returnValue(of(null));
    cookieServiceMock.get.and.returnValue(null);
    cookieServiceMock.check.and.returnValue(true);
    httpClientMock.post.and.returnValue(of(u));
    const service: AuthService = TestBed.inject(AuthService);
    service.IsAuthenticated().subscribe(result => {
      expect(result).toBe(true);
      expect(cookieServiceMock.check).toHaveBeenCalledWith('id');
      expect(cookieServiceMock.get).toHaveBeenCalledWith('id');
    });
  });

  it('should not be authenticated', () =>
  {
    userServiceMock.GetUserAsync.and.returnValue(of(null));
    cookieServiceMock.get.and.returnValue(null);
    cookieServiceMock.check.and.returnValue(false);
    const service: AuthService = TestBed.inject(AuthService);
    service.IsAuthenticated().subscribe(result => expect(result).toBe(false));
  });

  it('should get token', () =>
  {
    cookieServiceMock.get.and.returnValue('token');
    const service: AuthService = TestBed.inject(AuthService);
    expect(service.GetToken()).not.toBeNull();
  });

  it('should authenticate', () =>
  {
    let u: UserViewModel = {Avatar: "", Id: "", IsDarkMode: false, Token: "", UserName: ""};
    httpClientMock.post.and.returnValue(of(u));
    const service: AuthService = TestBed.inject(AuthService);
    service.Authenticate('legit id').subscribe(result => expect(result).not.toBeNull());
  });

  it('should login', async(() =>
  {
    let u: UserViewModel = {Avatar: "", Id: "", IsDarkMode: false, Token: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c", UserName: ""};
    httpClientMock.post.and.returnValue(of(u));
    const service: AuthService = TestBed.inject(AuthService);
    service.Login({Email: '', Password: '', Remember: false}).subscribe(result =>
    {
      expect(result).not.toBeNull();
      expect(cookieServiceMock.set).toHaveBeenCalled();
      expect(userServiceMock.User).not.toBeNull();
    });
  }));

  it('should change password', async (() =>
  {
    httpClientMock.post.and.returnValue(of(new HttpResponse({status: 200})))
    const service: AuthService = TestBed.inject(AuthService);
    service.UpdatePassword({Id: '', Password: ''}).subscribe(result =>
    {
      expect(result.status).toEqual(200);
      expect(result.body).toBeNull();
    });
  }));

});
