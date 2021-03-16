import {ComponentFixture, TestBed} from '@angular/core/testing';

import {PromosComponent} from './promos.component';
import {MatDialog} from "@angular/material/dialog";
import {TriggerService} from "../../../_Services/trigger.service";
import {PromoService} from "../../../_Services/promo.service";
import {of} from "rxjs";
import {CreatePromoComponent} from "./create-promo/create-promo.component";
import {IteratorStatus} from "../../../_extensions/iterator/iterator.component";
import {MatSlideToggleChange} from "@angular/material/slide-toggle";
import {Promo} from "../../../_models/Promo";

describe('PromosComponent', () =>
{
  let component: PromosComponent;
  let fixture: ComponentFixture<PromosComponent>;

  let dialogMock;
  let promoServiceMock;
  let triggerServiceMock;

  beforeEach(async () =>
  {

    promoServiceMock = jasmine.createSpyObj('PromoService', ['Destroy', 'Update']);
    promoServiceMock.Destroy.and.returnValue(of(null));
    promoServiceMock.Update.and.returnValue(of(null));

    triggerServiceMock = jasmine.createSpyObj('TriggerService', ['Iterate']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);
    dialogMock.open.and.returnValue({afterClosed: () => of(true)});

    await TestBed.configureTestingModule({
      declarations: [PromosComponent],
      providers: [
        {provide: MatDialog, useValue: dialogMock},
        {provide: TriggerService, useValue: triggerServiceMock},
        {provide: PromoService, useValue: promoServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(PromosComponent);
    component = fixture.componentInstance;

    component.Event = {
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
      Promos: [
        {Active: true, Discount: 0, End: new Date(), Id: "1", NumberOfSales: 0, Start: new Date()},
        {Active: false, Discount: 0, End: new Date(), Id: "2", NumberOfSales: 0, Start: new Date()}
      ],
      SocialLinks: [],
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined,
      Tickets: [],
      TicketsLeft: 0,
      Transactions: [],
      Finished: false,
      Created: new Date()
    }

    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should open create dialog', () =>
  {
    let promo: Promo = {Active: true, Discount: 0, End: new Date(), Id: "1", NumberOfSales: 0, Start: new Date()};
    dialogMock.open.and.returnValue({afterClosed: () => of(promo)});
    component.create();
    expect(dialogMock.open).toHaveBeenCalledWith(CreatePromoComponent, {
      data: {
        event: component.Event
      }
    });
    expect(component.Event.Promos).toContain(promo);
  });

  it('should delete promo', () =>
  {
    component.delete(component.Event.Promos[0]);
    expect(component.Loading).toBeFalse();
    expect(triggerServiceMock.Iterate).toHaveBeenCalledWith("Removed promo", 2000, IteratorStatus.good);
    expect(component.Event.Promos).toEqual([component.Event.Promos[0]]);
  });

  it('should activate promo', () =>
  {
    component.activeToggle({checked: true} as MatSlideToggleChange, component.Event.Promos[1]);
    expect(triggerServiceMock.Iterate).toHaveBeenCalledWith("Updated promo", 2000, IteratorStatus.good);
  });
});
