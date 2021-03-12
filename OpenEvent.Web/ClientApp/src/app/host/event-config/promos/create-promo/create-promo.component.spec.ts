import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';

import {CreatePromoComponent} from './create-promo.component';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {PromoService} from "../../../../_Services/promo.service";
import {of} from "rxjs";
import {TriggerService} from "../../../../_Services/trigger.service";
import {IteratorStatus} from "../../../../_extensions/iterator/iterator.component";

describe('CreatePromoComponent', () =>
{
  let component: CreatePromoComponent;
  let fixture: ComponentFixture<CreatePromoComponent>;

  let dialogRefMock;
  let promoServiceMock;
  let triggerServiceMock;

  beforeEach(async () =>
  {

    dialogRefMock = jasmine.createSpyObj('matDialogRef', ['close']);

    triggerServiceMock = jasmine.createSpyObj('TriggerService', ['Iterate']);
    triggerServiceMock.Iterate.and.returnValue(of(null));

    promoServiceMock = jasmine.createSpyObj('PromoService', ['Create']);
    promoServiceMock.Create.and.returnValue(of(null));

    await TestBed.configureTestingModule({
      declarations: [CreatePromoComponent],
      providers: [
        {provide: MAT_DIALOG_DATA, useValue: {}},
        {provide: MatDialogRef, useValue: dialogRefMock},
        {provide: PromoService, useValue: promoServiceMock},
        {provide: TriggerService, useValue: triggerServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(CreatePromoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should create promo', () =>
  {
    component.createPromoForm.setValue({
      active: true,
      discount: 50,
      end: new Date(new Date().getTime() + 1000),
      start: new Date()
    });
    component.data.event = {
      Address: undefined,
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Id: "EventId",
      Images: [],
      IsOnline: false,
      Name: "",
      Price: 0,
      Promos: [],
      SocialLinks: [],
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined,
      Tickets: [],
      TicketsLeft: 0,
      Transactions: []
    }
    component.create();
    expect(component.loading).toBeFalse();
    expect(triggerServiceMock.Iterate).toHaveBeenCalledWith("Created Promo", 2000, IteratorStatus.good);
    expect(dialogRefMock.close).toHaveBeenCalled();
  });
});
