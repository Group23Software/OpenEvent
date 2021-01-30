import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {UserService} from "../../_Services/user.service";

@Component({
  selector: 'user-nav',
  templateUrl: './user-nav.component.html',
  styleUrls: ['./user-nav.component.css']
})
export class UserNavComponent
{

  get User ()
  {
    return this.userService.User;
  }

  constructor (private userService: UserService, private router: Router)
  {
  }

  public logout ()
  {
    this.userService.LogOut();
    this.router.navigate(['/login']);
  }
}
