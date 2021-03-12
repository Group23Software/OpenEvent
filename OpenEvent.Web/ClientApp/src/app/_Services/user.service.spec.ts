import {async, TestBed} from '@angular/core/testing';

import {UserService} from './user.service';
import {CookieService} from "ngx-cookie-service";
import {
  NewUserInput,
  UpdateAvatarBody,
  UpdateThemePreferenceBody,
  UpdateUserNameBody,
  UserAccountModel,
  UserViewModel
} from "../_models/User";
import {HttpClient} from "@angular/common/http";
import {of} from "rxjs";
import any = jasmine.any;
import {FakeAddress} from "../_testData/Event";
import {UsersAnalytics} from "../_models/Analytic";

describe('UserService', () =>
{
  let httpClientMock;
  let cookieServiceMock;

  beforeEach(() =>
  {

    cookieServiceMock = jasmine.createSpyObj('cookieService', ['get', 'set', 'check', 'delete', 'deleteAll', 'getAll']);
    cookieServiceMock.get.and.returnValue('');
    cookieServiceMock.set.and.callThrough();
    cookieServiceMock.check.and.callThrough();

    httpClientMock = jasmine.createSpyObj('httpClient', ['post', 'get', 'delete']);
    httpClientMock.post.and.returnValue(jasmine.createSpyObj("post", ["subscribe"]));

    TestBed.configureTestingModule({
      imports: [],
      providers: [
        {provide: 'BASE_URL', useValue: ''},
        {provide: HttpClient, useValue: httpClientMock},
        {provide: CookieService, useValue: cookieServiceMock}
      ]
    })
  });

  it('should be created', () =>
  {
    const service: UserService = TestBed.inject(UserService);
    expect(service).toBeTruthy();
  });

  it('should get user async', async(() =>
  {
    const service: UserService = TestBed.inject(UserService);
    service.GetUserAsync().subscribe(u => expect(u).toEqual(service.User));
  }));

  it('should get account user', async(() =>
  {
    let aU: UserAccountModel = {
      Avatar: "",
      DateOfBirth: undefined,
      Email: "",
      FirstName: "",
      Id: "",
      IsDarkMode: false,
      LastName: "",
      PhoneNumber: "",
      UserName: ""
    };
    httpClientMock.get.and.returnValue(of(aU));
    const service: UserService = TestBed.inject(UserService);
    service.GetAccountUser(null).subscribe(u => expect(u).not.toBeNull());
  }));

  it('should create user', async(() =>
  {
    let u: UserViewModel = {
      Avatar: "",
      Id: "1",
      IsDarkMode: false,
      Token: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
      UserName: ""
    };
    let input: NewUserInput = {
      Avatar: [],
      DateOfBirth: undefined,
      Email: "",
      FirstName: "",
      LastName: "",
      Password: "",
      PhoneNumber: "",
      Remember: false,
      UserName: ""
    };

    httpClientMock.post.and.returnValue(of(u));

    const service: UserService = TestBed.inject(UserService);
    service.CreateUser(input).subscribe(result =>
    {
      expect(result).toEqual(u);
      expect(cookieServiceMock.set).toHaveBeenCalledTimes(2);
    });
  }));

  it('should destroy user', async(() =>
  {
    httpClientMock.delete.and.returnValue(of(any));
    const service: UserService = TestBed.inject(UserService);
    service.Destroy(null).subscribe(result =>
    {
      expect(cookieServiceMock.delete).toHaveBeenCalledTimes(2);
      expect(cookieServiceMock.deleteAll).toHaveBeenCalled();
      expect(service.User).toBeNull();
    });
  }));

  it('should update username', async(() =>
  {
    let u: UpdateUserNameBody = {Id: "1", UserName: "name"}
    let res = {username: u.UserName};
    httpClientMock.post.and.returnValue(of(res));
    const service: UserService = TestBed.inject(UserService);
    service.User = {Avatar: "", Id: "", IsDarkMode: false, UserName: ""}

    service.UpdateUserName(u).subscribe(result =>
    {
      expect(result.username).toEqual(u.UserName);
      expect(service.User.UserName).toEqual(u.UserName);
    })
  }));

  it('should update avatar', async(() =>
  {
    let u: UpdateAvatarBody = {Avatar: [0, 0, 0, 0], Id: ""}
    let res = {avatar: "0,0,0,0"}
    httpClientMock.post.and.returnValue(of(res));
    const service: UserService = TestBed.inject(UserService);
    service.User = {Avatar: "", Id: "", IsDarkMode: false, UserName: ""}

    service.UpdateAvatar(u).subscribe(result =>
    {
      expect(result.avatar).toEqual(res.avatar);
      expect(service.User.Avatar).toEqual(res.avatar);
    });
  }));

  it('should update theme preference', async(() =>
  {
    let u: UpdateThemePreferenceBody = {Id: "", IsDarkMode: true}
    let res = {isDarkMode: true}
    httpClientMock.post.and.returnValue(of(res));
    const service: UserService = TestBed.inject(UserService);
    service.User = {Avatar: "", Id: "", IsDarkMode: false, UserName: ""}

    service.UpdateThemePreference(u).subscribe(result =>
    {
      expect(result.isDarkMode).toEqual(u.IsDarkMode);
      expect(service.User.IsDarkMode).toEqual(u.IsDarkMode);
    });

  }));

  it('should log out', async(() =>
  {
    const service: UserService = TestBed.inject(UserService);
    service.LogOut();
    expect(cookieServiceMock.delete).toHaveBeenCalledTimes(2);
    expect(cookieServiceMock.deleteAll).toHaveBeenCalled();
    expect(service.User).toBeNull();
  }));

  it('username should exist', async(() =>
  {
    httpClientMock.get.and.returnValue(of(true));
    const service: UserService = TestBed.inject(UserService);
    service.UserNameExists("").subscribe(result => expect(result).toBe(true));
  }));

  it('email should exist', async(() =>
  {
    httpClientMock.get.and.returnValue(of(true));
    const service: UserService = TestBed.inject(UserService);
    service.EmailExists("").subscribe(result => expect(result).toBe(true));
  }));

  it('phone should exist', async(() =>
  {
    httpClientMock.get.and.returnValue(of(true));
    const service: UserService = TestBed.inject(UserService);
    service.PhoneExists("").subscribe(result => expect(result).toBe(true));
  }));

  it('should get detailed account if it does not exist', () =>
  {
    const service: UserService = TestBed.inject(UserService);
    let getSpy = spyOn(service, 'GetAccountUser');
    service.User = {Avatar: "", Id: "UserId", IsDarkMode: false, UserName: ""};
    service.NeedAccountUser();
    expect(getSpy).toHaveBeenCalled();
  });

  it('should update address', () =>
  {
    httpClientMock.post.and.returnValue(of({address: FakeAddress}));
    const service: UserService = TestBed.inject(UserService);
    service.User = {Avatar: "", Id: "UserId", IsDarkMode: false, UserName: ""};
    service.UpdateAddress({
      Id: "UserId",
      Address: FakeAddress
    }).subscribe();
    expect(service.User.Address).toEqual(FakeAddress);
  });

  it('should get analytics', () =>
  {
    httpClientMock.get.and.returnValue(of({
      PageViewEvents: [],
      RecommendationScores: [],
      SearchEvents: [],
      TicketVerificationEvents: []
    } as UsersAnalytics));
    const service: UserService = TestBed.inject(UserService);
    service.User = {Avatar: "", Id: "UserId", IsDarkMode: false, UserName: ""};
    service.GetAnalytics().subscribe(x => expect(x).not.toBeNull());
  });
});
