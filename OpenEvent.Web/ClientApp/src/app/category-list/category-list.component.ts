import {Component, Input, OnInit, Output} from '@angular/core';
import {Category} from "../_models/Category";
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit
{
  @Input() public inset: boolean = true;
  @Input() public categories: Category[] = [];
  @Input() public selectedCategories: Category[] = [];
  @Output() categoryEvent = new EventEmitter<Category[]>();

  constructor ()
  {
  }

  ngOnInit (): void
  {
  }

  public addCategory (category: Category)
  {
    this.selectedCategories.push(category);
    this.categories = this.categories.filter(x => x.Id != category.Id);
    this.categoryEvent.emit(this.selectedCategories);
  }

  public removeCategory (category: Category)
  {
    this.categories.push(category);
    this.selectedCategories = this.selectedCategories.filter(x => x.Id != category.Id);
    this.categoryEvent.emit(this.selectedCategories);
  }
}
