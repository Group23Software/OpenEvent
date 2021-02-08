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

  constructor (private userService: UserService, private router: Router, private trigger: TriggerService, private snackBar: MatSnackBar, private location: Location)
  {
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
    console.log('toggling theme',!isDark);
    this.trigger.isDark.emit(!isDark);
    this.userService.UpdateThemePreference({
      Id: this.User.Id,
      IsDarkMode: !isDark
    }).subscribe(response => {
      this.snackBar.open('Updated theme preference', 'close', {duration: 500});
    }, (error: HttpErrorResponse) => {
      console.error(error);
    });
  }

  public back ()
  {
    this.location.back();
  }
}
