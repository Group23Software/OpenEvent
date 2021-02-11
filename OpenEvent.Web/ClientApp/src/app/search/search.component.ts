import {Component, OnInit} from '@angular/core';
import {Subject, Subscription} from "rxjs";
import {debounceTime} from "rxjs/operators";
import {EventService} from "../_Services/event.service";
import {EventViewModel, SearchFilter, SearchParam} from "../_models/Event";
import {Category} from "../_models/Category";
import {MatSlideToggleChange} from "@angular/material/slide-toggle";

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
  public categories: Category[] = [];
  public selectedCategories: Category[] = [];
  private usersLocation: Position;
  public minDate: Date = new Date;
  public usingCurrentLocation: boolean = false;
  public distanceSelect: string;
  public loading: boolean = false;
  public date: Date;
  public usingDate: boolean = false;

  constructor (private eventService: EventService)
  {
  }

  ngOnInit (): void
  {
    this.searchSubscription = this.searchChanged.pipe(debounceTime(500),).subscribe(() =>
    {
      this.search();
    });

    this.eventService.GetAllCategories().subscribe(c => this.categories = c);
  }

  public search (): void
  {
    this.loading = true;

    let filters: SearchFilter[] = [];
    this.selectedCategories.forEach(c => filters.push({Key: SearchParam.Category, Value: c.Id}));
    if (this.isOnline)
    {
      filters.push({Key: SearchParam.IsOnline, Value: "true"});
    }

    if (this.usersLocation && this.usingCurrentLocation && !this.isOnline) filters.push({
      Key: SearchParam.Location,
      Value: `${this.usersLocation.coords.latitude},${this.usersLocation.coords.longitude},${this.distanceSelect}`
    })

    if (this.date) filters.push({Key:SearchParam.Date, Value: this.date.toDateString()})

    console.log(filters);
    this.eventService.Search(this.keyword, filters).subscribe(events =>
    {
      this.events = events;
      this.loading = false;
    });
  }

  public triggerSearchChange ()
  {
    console.log('trigger');
    this.searchChanged.next();
  }

  public addCategory (category: Category)
  {
    this.selectedCategories.push(category);
    this.categories = this.categories.filter(x => x.Id != category.Id);
  }

  public removeCategory (category: Category)
  {
    this.categories.push(category);
    this.selectedCategories = this.selectedCategories.filter(x => x.Id != category.Id);
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
