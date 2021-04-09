import {Component, OnInit} from '@angular/core';
import {PopularityService} from "../_Services/popularity.service";
import {forkJoin} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'],
})
export class LandingComponent implements OnInit
{
  public Error: string;
  public Loading: boolean = true;

  get FeaturedEvent() {
    return this.popularityService.PopularEvents[0];
  }

  get PopularEvents ()
  {
    return this.popularityService.PopularEvents;
  }

  get PopularCategories ()
  {
    return this.popularityService.PopularCategories;
  }

  constructor (private popularityService: PopularityService, private router: Router)
  {
  }

  ngOnInit (): void
  {
    this.Loading = true;
    this.popularityService.ListenToEvents();
    this.popularityService.ListenToCategories();

    let subs = [
      this.popularityService.GetEvents(),
      this.popularityService.GetCategories()
    ];
    forkJoin(subs).subscribe(x =>
    {
    }, (e: HttpErrorResponse) => this.Error = e.error, () => {
      this.Loading = false;
      // console.log('finished loading', this.Loading);
    });
  }

  public navigateToFeatured ()
  {
    this.router.navigate(['/event',this.FeaturedEvent.Id]);
  }
}
