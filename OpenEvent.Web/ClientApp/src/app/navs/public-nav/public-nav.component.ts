import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {Location} from "@angular/common";

@Component({
  selector: 'public-nav',
  templateUrl: './public-nav.component.html',
  styleUrls: ['./public-nav.component.css']
})
export class PublicNavComponent implements OnInit {

  constructor(private router: Router, private location: Location) { }

  ngOnInit() {
  }

  routeHome ()
  {
    this.router.navigate(['/']);
  }

  public back ()
  {
    this.location.back();
  }
}
