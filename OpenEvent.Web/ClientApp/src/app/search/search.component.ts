import {Component, OnInit} from '@angular/core';
import {Subject, Subscription} from "rxjs";
import {debounceTime} from "rxjs/operators";
import {EventService} from "../_Services/event.service";
import {EventViewModel, SearchFilter, SearchParam} from "../_models/Event";
import {Category} from "../_models/Category";
import {MatSlideToggleChange} from "@angular/material/slide-toggle";
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit
{

  private searchChanged: Subject<string> = new Subject<string>();
  private searchSubscription: Subscription;

  public isOnline: boolean = false;
  public events: EventViewModel[];
  public keyword: string = '';
  public filters: SearchFilter[] = [];
  public categories: Category[] = [];
  public selectedCategories: Category[] = [];
  public getCategoriesError: string;
  public searchError: string;
  public usersLocation: Position;
  public minDate: Date = new Date;
  public usingCurrentLocation: boolean = false;
  public distanceSelect: string;
  public loading: boolean = false;
  public date: Date;
  public usingDate: boolean = false;

  constructor (private eventService: EventService, private router: Router)
  {
    let search = this.router.getCurrentNavigation().extras.state as {keyword: string,filters: SearchFilter[]};
    if (search) {
      console.log(search);
      this.keyword = search.keyword;
      this.filters = search.filters;
    }
  }

  ngOnInit (): void
  {
    this.searchSubscription = this.searchChanged.pipe(debounceTime(500),).subscribe(() =>
    {
      this.search();
    });

    this.eventService.GetAllCategories().subscribe(c => this.categories = c,(e: HttpErrorResponse) => this.getCategoriesError = e.error.Message);

    if (this.filters != [] || this.keyword != '') {
      this.search();
    }
  }

  public search (): void
  {
    this.loading = true;
    this.selectedCategories.forEach(c => this.filters.push({Key: SearchParam.Category, Value: c.Id}));
    if (this.isOnline)
    {
      this.filters.push({Key: SearchParam.IsOnline, Value: "true"});
    }

    if (this.usersLocation && this.usingCurrentLocation && !this.isOnline) this.filters.push({
      Key: SearchParam.Location,
      Value: `${this.usersLocation.coords.latitude},${this.usersLocation.coords.longitude},${this.distanceSelect}`
    })

    if (this.date && this.usingDate) this.filters.push({Key:SearchParam.Date, Value: this.date.toDateString()})

    console.log(this.filters);
    this.eventService.Search(this.keyword, this.filters).subscribe(events =>
    {
      this.events = events;
      this.loading = false;
    }, (e: HttpErrorResponse) => {
      this.loading = false;
      this.searchError = e.error.Message;
    });
  }

  public triggerSearchChange ()
  {
    this.searchChanged.next();
  }

  public toggleCurrentLocation (event: MatSlideToggleChange)
  {
    console.log(event.checked);
    if (event.checked)
    {
      if (navigator.geolocation)
      {
        console.log("Geo location supported");
        navigator.geolocation.getCurrentPosition((position) =>
        {
          this.usersLocation = position;
        }, positionError => console.error(positionError), {timeout: 10000});
      } else
      {
        console.log("Geo location is not supported");
      }
    }
  }
}
