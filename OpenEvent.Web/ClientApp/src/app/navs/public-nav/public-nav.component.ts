import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'public-nav',
  templateUrl: './public-nav.component.html',
  styleUrls: ['./public-nav.component.css']
})
export class PublicNavComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  routeHome ()
  {
    this.router.navigate(['/']);
  }
}
