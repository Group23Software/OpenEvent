import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Address} from "../_models/Address";

@Component({
  selector: 'address-form',
  templateUrl: './address-form.component.html',
  styleUrls: ['./address-form.component.css']
})
export class AddressFormComponent implements OnInit
{
  @Input() Address: Address;
  @Input() Disabled: boolean = false;
  @Output() SubmitEvent: EventEmitter<Address> = new EventEmitter<Address>();

  public addressForm = new FormGroup({
    AddressLine1: new FormControl('', [Validators.required]),
    AddressLine2: new FormControl(''),
    City: new FormControl('', [Validators.required]),
    PostalCode: new FormControl('', [Validators.required, Validators.pattern('([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\\s?[0-9][A-Za-z]{2})')]),
    CountryCode: new FormControl('GB'),
    CountryName: new FormControl('United Kingdom'),
    Lat: new FormControl(),
    Lon: new FormControl()
  });

  constructor ()
  {
  }

  ngOnInit (): void
  {
    if (this.Disabled)
    {
      for (let control in this.addressForm.controls)
      {
        this.addressForm.controls[control].disable();
      }
    }

    if (this.Address)
    {
      this.addressForm.setValue(this.Address);
    }
  }

  public submit (): void
  {
    this.SubmitEvent.emit({
      AddressLine1: this.addressForm.controls.AddressLine1.value,
      AddressLine2: this.addressForm.controls.AddressLine2.value,
      City: this.addressForm.controls.City.value,
      CountryCode: this.addressForm.controls.CountryCode.value,
      CountryName: this.addressForm.controls.CountryName.value,
      PostalCode: this.addressForm.controls.PostalCode.value
    } as Address)
  }
}
