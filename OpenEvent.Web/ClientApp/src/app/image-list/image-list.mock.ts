import {ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR} from "@angular/forms";
import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {ImageViewModel} from "../_models/Image";

@Component({
  selector: 'image-list',
  template: '',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: MockImageListComponent
    }
  ]
})
export class MockImageListComponent implements ControlValueAccessor, OnInit
{

  @Input() images: ImageViewModel[] = [];
  @Output() public imageEvent = new EventEmitter<ImageViewModel[]>();

  ngOnInit (): void
  {
  }

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
