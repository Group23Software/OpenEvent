import {Component, OnInit} from '@angular/core';
import {UserService} from "../_Services/user.service";
import {HttpErrorResponse} from "@angular/common/http";


@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit
{

  public userLoaded: boolean = false;

  constructor (
    private userService: UserService)
  {
  }

  ngOnInit ()
  {
    this.userService.GetAccountUser(this.userService.User.Id).subscribe(response => this.userLoaded = true, (error: HttpErrorResponse) =>
    {
      // TODO: Get account user error handle, Redirect?
    });
  }
}
