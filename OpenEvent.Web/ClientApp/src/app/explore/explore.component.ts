import {Component, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {EventService} from "../_Services/event.service";
import {Category} from "../_models/Category";
import {InOutAnimation} from "../_extensions/animations";
import {map} from "rxjs/operators";
import {EventViewModel, SearchParam} from "../_models/Event";
import {forkJoin} from "rxjs";
import {UserService} from "../_Services/user.service";
import {MatChip, MatChipList} from "@angular/material/chips";
import {HttpErrorResponse} from "@angular/common/http";
import {TriggerService} from "../_Services/trigger.service";
import {PopularityService} from "../_Services/popularity.service";

@Component({
  selector: 'app-explore',
  templateUrl: './explore.component.html',
  styleUrls: ['./explore.component.css'],
  animations: InOutAnimation
})
export class ExploreComponent implements OnInit
{
  @ViewChild(MatChipList) categoryList: MatChipList;
  @ViewChildren(MatChip) chips: QueryList<MatChip>;

  public Categories: Category[];
  public SelectedCategories: Category[] = [];
  public Events: EventViewModel[];
  public Error: string;
  public DisplayingExplore: boolean = true;

  get PopularEvents ()
  {
    return this.popularityService.PopularEvents;
  }

  get PopularCategories ()
  {
    return this.popularityService.PopularCategories;
  }

  constructor (private eventService: EventService, private userService: UserService, private trigger: TriggerService, private popularityService: PopularityService)
  {
  }

  ngOnInit ()
  {
    let subs = [
      this.eventService.GetAllCategories().pipe(map(categories => this.Categories = categories)),
      this.eventService.Explore().pipe(map(events => this.Events = events)),
      this.popularityService.GetEvents(),
      this.popularityService.GetCategories()
    ];
    forkJoin(subs).subscribe(x => {
      // console.log("explore data loaded", x);
    }, (e: HttpErrorResponse) => this.Error = e.message);
  }

  public explore ()
  {
    this.eventService.Explore().subscribe(events => this.Events = events);
    this.SelectedCategories = [];
    this.chips.forEach(chip => chip.deselect())
  }

  public categoriesSelectionChange (chip: MatChip, category: Category)
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
