import {Component, OnInit} from '@angular/core';
import {EventService} from "../_Services/event.service";
import {CategoryViewModel} from "../_models/Category";
import {InOutAnimation} from "../_extensions/animations";
import {map, tap} from "rxjs/operators";
import {EventViewModel, SearchParam} from "../_models/Event";
import {forkJoin} from "rxjs";
import {UserService} from "../_Services/user.service";
import {MatChip, MatChipListChange, MatChipSelectionChange} from "@angular/material/chips";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-explore',
  templateUrl: './explore.component.html',
  styleUrls: ['./explore.component.css'],
  animations: InOutAnimation
})
export class ExploreComponent implements OnInit
{

  public Categories: CategoryViewModel[];
  public SelectedCategories: CategoryViewModel[] = [];
  public Events: EventViewModel[];
  public Error: string;
  private DisplayingExplore: boolean = true;

  get PopularEvents ()
  {
    return new Array<number>(10);
  }

  constructor (private eventService: EventService, private userService: UserService)
  {
  }

  ngOnInit ()
  {
    let subs = [
      this.eventService.GetAllCategories().pipe(map(categories => this.Categories = categories)),
      this.eventService.Explore().pipe(map(events => this.Events = events))
    ];
    forkJoin(subs).subscribe(x => console.log("explore data loaded", x, (e: HttpErrorResponse) => this.Error = e.message));
    // this.eventService.GetAllCategories().subscribe(x => this.Categories = x);
    // this.eventService.Explore().subscribe(x => this.Events = x);
  }

  public explore ()
  {
    this.eventService.Explore().subscribe(events => this.Events = events);
  }

  public categoriesSelectionChange (chip: MatChip, category: CategoryViewModel)
  {
    chip.toggleSelected();
    if (chip.selected)
    {
      this.SelectedCategories.push(category);
    } else
    {
      this.SelectedCategories = this.SelectedCategories.filter(x => x != category);
    }

    if (this.SelectedCategories.length == 0)
    {
      if (!this.DisplayingExplore) this.eventService.Explore().subscribe(events => this.Events = events);
      this.DisplayingExplore = true;
    } else
    {
      this.DisplayingExplore = false;
      this.eventService.Search('', this.SelectedCategories.map(c => ({
        Key: SearchParam.Category,
        Value: c.Id
      }))).subscribe(events => this.Events = events, (e: HttpErrorResponse) => this.Error = e.message);
    }
  }
}
