import {TestBed, async, inject, getTestBed} from '@angular/core/testing';

import {AuthGuard} from './auth.guard';
import {AuthService} from "../_Services/auth.service";
import {Router} from "@angular/router";
import {of} from "rxjs";

describe('AuthGuard', () =>
{

  let authServiceMock;
  let routerMock;
  let injector: TestBed;
  let guard: AuthGuard;

  beforeEach(() =>
  {

    authServiceMock = jasmine.createSpyObj('authService', ['IsAuthenticated']);

    routerMock = jasmine.createSpyObj('router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        {provide: AuthService, useValue: authServiceMock},
        {provide: Router, useValue: routerMock},
        AuthGuard
      ]
    });
    injector = getTestBed();
    guard = injector.get(AuthGuard);
  });

  it('should be created', inject([AuthGuard], (guard: AuthGuard) =>
  {
    expect(guard).toBeTruthy();
  }));

  it('should allow route', inject([AuthGuard], (guard: AuthGuard) =>
  {
    authServiceMock.IsAuthenticated.and.returnValue(of(true));
    guard.canActivate(null,null).subscribe(result => {
      expect(result).toBeTruthy();
    });
  }));

  it('should not allow route', inject([AuthGuard], (guard: AuthGuard) =>
  {
    authServiceMock.IsAuthenticated.and.returnValue(of(false));
    guard.canActivate(null,null).subscribe(result => {
      expect(result).toBeFalsy();
      expect(routerMock.navigate).toHaveBeenCalledWith(['/login']);
    });
  }));
});
