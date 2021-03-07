import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressFormComponent } from './address-form.component';
import {FakeAddress} from "../_testData/Event";
import {EventEmitter} from "@angular/core";
import {Address} from "../_models/Address";

class triggerServiceStub {
  public loading: EventEmitter<Address> = new EventEmitter<Address>();
}

describe('AddressFormComponent', () => {
  let component: AddressFormComponent;
  let fixture: ComponentFixture<AddressFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
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
