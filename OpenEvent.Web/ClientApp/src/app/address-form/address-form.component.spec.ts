import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressFormComponent } from './address-form.component';
import {FakeAddress} from "../_testData/Event";
import {EventEmitter} from "@angular/core";
import {Address} from "../_models/Address";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

class triggerServiceStub {
  public loading: EventEmitter<Address> = new EventEmitter<Address>();
}

describe('AddressFormComponent', () => {
  let component: AddressFormComponent;
  let fixture: ComponentFixture<AddressFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        MatFormFieldModule,
        MatInputModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule
      ],
      declarations: [ AddressFormComponent ],
      providers: []
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should disable form controls', () =>
  {
    component.setDisabledState(true);
    component.ngOnInit();
    expect(component.addressForm.disabled).toBeTruthy();
  });

  it('should load address into form', () =>
  {
    component.setDisabledState(false);
    component.Address = FakeAddress;
    component.ngOnInit();
    expect(component.addressForm.value).toEqual(FakeAddress);
  });
});
