import {Component, Input, Output} from '@angular/core';
import {Category} from "../_models/Category";
import {EventEmitter} from '@angular/core';
import {InOutAnimation} from "../_extensions/animations";
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR
} from "@angular/forms";

@Component({
  selector: 'category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css'],
  animations: InOutAnimation,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: CategoryListComponent
    }
  ]
})
export class CategoryListComponent implements ControlValueAccessor
{
  @Input() public inset: boolean = true;
  @Input() public categories: Category[] = [];
  @Input() public selectedCategories: Category[] = [];
  @Output() categoryEvent = new EventEmitter<Category[]>();

  onChange = (categories) =>
  {
  };

  onTouched = () =>
  {
  };

  disabled: boolean = false;

  constructor ()
  {
  }

  public touched: boolean = false;

  public addCategory (category: Category)
  {
    if (!this.touched) this.touched = true;
    this.selectedCategories.push(category);
    this.categories = this.categories.filter(x => x.Id != category.Id);
    this.categoryEvent.emit(this.selectedCategories);
    this.onChange(this.selectedCategories);
  }

  public removeCategory (category: Category)
  {
    if (!this.touched) this.touched = true;
    this.categories.push(category);
    this.selectedCategories = this.selectedCategories.filter(x => x.Id != category.Id);
    this.categoryEvent.emit(this.selectedCategories);
    this.onChange(this.selectedCategories);
  }

  registerOnChange (fn: any): void
  {
    this.onChange = fn;
  }

  registerOnTouched (fn: any): void
  {
    this.onTouched = fn;
  }

  setDisabledState (isDisabled: boolean): void
  {
    this.disabled = isDisabled;
  }

  writeValue (categories: Category[]): void
  {
    if (categories.length == 0) this.selectedCategories = [];
    else this.selectedCategories = categories;
  }
}
