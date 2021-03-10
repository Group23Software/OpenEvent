import {ComponentFixture, TestBed} from '@angular/core/testing';

import {PromosComponent} from './promos.component';
import {MatDialog} from "@angular/material/dialog";
import {TriggerService} from "../../../_Services/trigger.service";
import {PromoService} from "../../../_Services/promo.service";

describe('PromosComponent', () =>
{
  let component: PromosComponent;
  let fixture: ComponentFixture<PromosComponent>;

  let dialogMock;
  let promoServiceMock;
  let triggerServiceMock;

  beforeEach(async () =>
  {

    promoServiceMock = jasmine.createSpyObj('PromoService',['Destroy','Update']);
    triggerServiceMock = jasmine.createSpyObj('TriggerService', ['Iterate']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open'])

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
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
