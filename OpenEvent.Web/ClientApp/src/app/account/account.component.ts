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
  public getUserError: string;
  public userLoaded: boolean = false;

  constructor (
    private userService: UserService)
  {
  }

  ngOnInit ()
  {
    this.userService.GetAccountUser(this.userService.User.Id).subscribe(user =>
    {
      this.userLoaded = true;
    }, (error: HttpErrorResponse) =>
    {
      this.getUserError = error.error.Message;
    });
  }
}
