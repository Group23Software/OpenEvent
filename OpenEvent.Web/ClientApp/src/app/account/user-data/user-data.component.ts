import {Component, OnInit} from '@angular/core';
import {UserService} from "../../_Services/user.service";
import {HttpErrorResponse} from "@angular/common/http";
import {MappedUsersAnalytics} from "../../_models/Analytic";

@Component({
  selector: 'user-data',
  templateUrl: './user-data.component.html',
  styleUrls: ['./user-data.component.css']
})
export class UserDataComponent implements OnInit
{

  public analytics: MappedUsersAnalytics
  recommendationColumns = ['name','weight'];

  constructor (private userService: UserService)
  {
  }

  ngOnInit (): void
  {
    this.userService.GetAnalytics().subscribe(analytics =>
    {
      this.analytics = analytics;
    }, (e: HttpErrorResponse) =>
    {
      console.error(e);
    })
  }

}
