import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageUploadComponent } from './image-upload.component';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

describe('ImageUploadComponent', () => {
  let component: ImageUploadComponent;
  let fixture: ComponentFixture<ImageUploadComponent>;

  let dialogRefMock;

  beforeEach(async () => {

    dialogRefMock = jasmine.createSpyObj('matDialogRef', ['close']);

    await TestBed.configureTestingModule({
      declarations: [ ImageUploadComponent ],
      providers:[
        {provide: MAT_DIALOG_DATA, useValue: {}},
        {provide: MatDialogRef, useValue: dialogRefMock},
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
