import {Component, OnInit} from "@angular/core";
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from "@angular/forms";

@Component({
  selector: 'social-links-form',
  template: '',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: MockSocialLinksFormComponent
    },
    // {
    //   provide: NG_VALIDATORS,
    //   useExisting: SocialLinksFormComponent,
    //   multi: true
    // }
  ]
})
export class MockSocialLinksFormComponent implements ControlValueAccessor, OnInit
{
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
