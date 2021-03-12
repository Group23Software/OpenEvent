import {Component, Input, OnInit} from '@angular/core';
import {CategoryViewModel, PopularCategoryViewModel} from "../../_models/Category";
import {SearchFilter, SearchParam} from "../../_models/Event";
import {Router} from "@angular/router";

@Component({
  selector: 'popular-categories',
  templateUrl: './popular-categories.component.html',
  styleUrls: ['./popular-categories.component.css']
})
export class PopularCategoriesComponent implements OnInit
{

  @Input() PopularCategories: PopularCategoryViewModel[];

  constructor (private router: Router)
  {
  }

  ngOnInit (): void
  {
  }

  public navigateToCategory (category: CategoryViewModel): void
  {
    this.router.navigateByUrl('/search', {
      state: {
        keyword: "",
        filters: [{Key: SearchParam.Category, Value: category.Id}] as SearchFilter[]
      }
    })
  }

}
