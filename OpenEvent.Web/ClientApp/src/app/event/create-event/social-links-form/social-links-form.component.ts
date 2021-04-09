import {Component, OnInit} from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  Validators
} from "@angular/forms";
import {SocialMedia} from "../../../_models/SocialMedia";
import {SocialLinkViewModel} from "../../../_models/SocialLink";

@Component({
  selector: 'social-links-form',
  templateUrl: './social-links-form.component.html',
  styleUrls: ['./social-links-form.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: SocialLinksFormComponent
    },
    // {
    //   provide: NG_VALIDATORS,
    //   useExisting: SocialLinksFormComponent,
    //   multi: true
    // }
  ]
})
export class SocialLinksFormComponent implements ControlValueAccessor, OnInit
{
  public optionsStore: string[] = Object.values(SocialMedia).filter(val => isNaN(Number(val))).map(key => key.toString());
  public optionsLeft: string[] = this.optionsStore;
  public optionsSelected: SocialLinkViewModel[] = []

  public socialLinkForm = new FormGroup({
    socialMedia: new FormControl(this.optionsStore[0], [Validators.required]),
    url: new FormControl('', [Validators.required]),
  });

  public isValid;

  onChange = (optionsSelected) =>
  {
  };

  onTouched = () =>
  {
  };

  constructor ()
  {
  }

  ngOnInit (): void
  {
  }

  registerOnChange (fn: any): void
  {
    this.onChange = fn;
  }

  registerOnTouched (fn: any): void
  {
    this.onTouched = fn;
  }

  writeValue (selected: SocialLinkViewModel[]): void
  {
    if (selected != null && selected.length > 0) this.optionsSelected = selected;
  }

  addSocialLink ()
  {
    this.optionsLeft = this.optionsLeft.filter(x => x != this.socialLinkForm.controls.socialMedia.value);
    this.optionsSelected.push({
      Link: this.socialLinkForm.controls.url.value,
      SocialMedia: SocialMedia[this.socialLinkForm.controls.socialMedia.value]
    });
    this.socialLinkForm.reset({
      socialMedia: this.optionsLeft[0],
      url: ''
    });

    this.onChange(this.optionsSelected);
  }

  removeSocialLink (social: SocialLinkViewModel)
  {
    this.optionsLeft.push(SocialMedia[social.SocialMedia]);
    this.optionsSelected = this.optionsSelected.filter(x => x != social);

    this.onChange(this.optionsSelected);
  }

  // validate({ value }: FormControl)
  // {
  //   if (value == null || value.length == 0) {
  //     this.isValid = false;
  //     return {required : true}
  //   }
  //
  //   console.log('valid checking',value);
  // }
}
