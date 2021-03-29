import {Component, Input} from "@angular/core";
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from "@angular/forms";

@Component({
  selector: 'address-form',
  template: '',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: MockAddressFormComponent
    }
  ]
})
export class MockAddressFormComponent implements ControlValueAccessor
{
  @Input() Address: any;

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
