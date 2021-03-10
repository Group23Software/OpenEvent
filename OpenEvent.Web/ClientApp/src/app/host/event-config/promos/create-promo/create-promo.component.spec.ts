import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePromoComponent } from './create-promo.component';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {PromoService} from "../../../../_Services/promo.service";

describe('CreatePromoComponent', () => {
  let component: CreatePromoComponent;
  let fixture: ComponentFixture<CreatePromoComponent>;

  let dialogRefMock;
  let promoServiceMock;

  beforeEach(async () => {

    dialogRefMock = jasmine.createSpyObj('matDialogRef', ['close']);


    await TestBed.configureTestingModule({
      declarations: [ CreatePromoComponent ],
      providers:[
        {provide: MAT_DIALOG_DATA, useValue: {}},
        {provide: MatDialogRef, useValue: dialogRefMock},
        {provide: PromoService, useValue: promoServiceMock}
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreatePromoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
