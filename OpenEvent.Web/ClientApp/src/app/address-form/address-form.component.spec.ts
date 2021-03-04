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
  let trigger;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddressFormComponent ],
      providers: []
    })
    .compileComponents();
  });

  beforeEach(() => {
    // trigger = TestBed.inject(TriggerService);
    fixture = TestBed.createComponent(AddressFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should disable form controls', () =>
  {
    component.Disabled = true;
    component.ngOnInit();
    for (let control in component.addressForm.controls)
    {
      expect(component.addressForm.controls[control].disabled).toBeTrue();
    }
  });

  it('should load address into form', () =>
  {
    component.Disabled = false;
    component.Address = FakeAddress;
    component.ngOnInit();
    expect(component.addressForm.value).toEqual(FakeAddress);
  });

  it('should emit address', () =>
  {
    component.addressForm.setValue(FakeAddress);
    let eventSpy = spyOn(component.SubmitEvent,'emit');
    component.submit();
    expect(eventSpy).toHaveBeenCalledWith(FakeAddress);
  });
});
