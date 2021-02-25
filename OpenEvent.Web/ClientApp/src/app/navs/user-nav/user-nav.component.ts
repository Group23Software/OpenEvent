import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {UserService} from "../../_Services/user.service";
import {TriggerService} from "../../_Services/trigger.service";
import {HttpErrorResponse} from "@angular/common/http";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Location} from "@angular/common";

@Component({
  selector: 'user-nav',
  templateUrl: './user-nav.component.html',
  styleUrls: ['./user-nav.component.css']
})
export class UserNavComponent
{
  public loading: boolean;

  constructor (
    private userService: UserService,
    private router: Router,
    private trigger: TriggerService,
    private snackBar: MatSnackBar,
    private location: Location)
  {
    trigger.loading.subscribe(loading => this.loading = loading);
  }

  get User ()
  {
    return this.userService.User;
  }

  public logout (): void
  {
    this.userService.LogOut();
    this.router.navigate(['/login']);
  }

  public routeHome (): void
  {
    this.router.navigate(['/']);
  }

  public toggleTheme (isDark: boolean): void
  {
    this.trigger.isDark.emit(!isDark);
    this.userService.UpdateThemePreference({
      Id: this.User.Id,
      IsDarkMode: !isDark
    }).subscribe(response =>
    {
      this.snackBar.open('Updated theme preference', 'close', {duration: 500});
    }, (error: HttpErrorResponse) =>
    {
      this.snackBar.open(error.error.Message, 'close', {duration: 1000})
    });
  }

  public back ()
  {
    this.location.back();
  }
}
