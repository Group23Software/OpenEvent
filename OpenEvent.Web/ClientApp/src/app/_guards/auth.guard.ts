import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router} from '@angular/router';
import {Observable} from 'rxjs';
import {AuthService} from "../_Services/auth.service";
import {tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate
{

  constructor (private authService: AuthService, private router: Router)
  {
  }

  canActivate (
    _next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean
  {
    console.log("Checking auth state");
    return this.authService.IsAuthenticated().pipe(tap(isAuthenticated =>
    {
      console.log(isAuthenticated);
      if (!isAuthenticated) this.router.navigate(['/login'])
      return isAuthenticated;
    }));
  }

}
