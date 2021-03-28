import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR} from "@angular/forms";
import {ImageViewModel} from "../_models/Image";

@Component({
  selector: 'thumbnail-edit',
  template: '',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: MockThumbnailEditComponent
    },
    {
      provide: NG_VALIDATORS,
      useExisting: MockThumbnailEditComponent,
      multi: true
    }
  ]
})
export class MockThumbnailEditComponent implements ControlValueAccessor, OnInit
{

  @Input() public thumbnail: ImageViewModel;
  @Output() public thumbnailEvent = new EventEmitter<ImageViewModel>();

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
