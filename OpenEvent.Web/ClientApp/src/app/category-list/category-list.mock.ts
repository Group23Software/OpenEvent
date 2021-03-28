import {Component, EventEmitter, forwardRef, Input, Output} from "@angular/core";
import {InOutAnimation} from "../_extensions/animations";
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from "@angular/forms";
import {Category} from "../_models/Category";

@Component({
  selector: 'category-list',
  template: '',
  animations: InOutAnimation,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => MockCategoryListComponent)
    }
  ]
})
export class MockCategoryListComponent implements ControlValueAccessor
{
  @Input() public inset: boolean = true;
  @Input() public categories: Category[] = [];
  @Input() public selectedCategories: Category[] = [];
  @Output() categoryEvent = new EventEmitter<Category[]>();

  registerOnChange (fn: any): void
  {
  }

  registerOnTouched (fn: any): void
  {
  }

  writeValue (obj: any): void
  {
  }
}
