import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormControl,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR, ValidationErrors, Validator,
  Validators
} from "@angular/forms";
import {Address} from "../_models/Address";
import {Subscription} from "rxjs";

@Component({
  selector: 'address-form',
  templateUrl: './address-form.component.html',
  styleUrls: ['./address-form.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: AddressFormComponent
    },
    {
      provide: NG_VALIDATORS,
      multi: true,
      useExisting: AddressFormComponent
    },
  ]
})
export class AddressFormComponent implements ControlValueAccessor, OnDestroy, Validator, OnInit
{
  @Input() Address: Address;
  // @Input() Disabled: boolean = false;
  @Output() SubmitEvent: EventEmitter<Address> = new EventEmitter<Address>();

  public addressForm = new FormGroup({
    AddressLine1: new FormControl('', [Validators.required]),
    AddressLine2: new FormControl(''),
    City: new FormControl('', [Validators.required]),
    PostalCode: new FormControl('', [Validators.required, Validators.pattern('([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\\s?[0-9][A-Za-z]{2})')]),
    CountryCode: new FormControl('GB'),
    CountryName: new FormControl('United Kingdom'),
    Lat: new FormControl(0),
    Lon: new FormControl(0)
  });

  addressChangeSubscriptions: Subscription[] = [];

  onTouched = () => {};

  touched = false;

  constructor ()
  {
  }

  ngOnInit (): void
  {
    if (this.Address)
    {
      this.addressForm.setValue(this.Address);
      this.addressForm.disable();
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
      PostalCode: this.addressForm.controls.PostalCode.value,
      Lon: 0,
      Lat: 0
    } as Address)
  }

  ngOnDestroy() {
    for (let sub of this.addressChangeSubscriptions) {
      sub.unsubscribe();
    }
  }

  registerOnChange (fn: any): void
  {
    this.addressChangeSubscriptions.push(this.addressForm.valueChanges.subscribe(fn));
  }

  registerOnTouched (fn: any): void
  {
    this.onTouched = fn;
  }

  setDisabledState (isDisabled: boolean): void
  {
    if (isDisabled) this.addressForm.disable();
    else this.addressForm.enable();
  }

  writeValue (address: Address): void
  {
    if (address) this.addressForm.setValue(address);
  }

  validate (control: AbstractControl): ValidationErrors | null
  {
   if (this.addressForm.valid) return null;
   return {'required': true};
  }
}
