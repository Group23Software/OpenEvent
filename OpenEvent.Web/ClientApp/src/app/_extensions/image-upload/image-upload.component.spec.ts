import {ComponentFixture, TestBed, tick} from '@angular/core/testing';

import { ImageUploadComponent } from './image-upload.component';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {By} from "@angular/platform-browser";

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

  it('should disable label when file changed',  () =>
  {
    component.fileChangeEvent("Event");
    expect(component.imageChangedEvent).toEqual("Event");
    expect(component.ImageLabel.disabled).toBeTrue();
  });

  it('should enable label input when ready',  () =>
  {
    component.cropperReady();
    expect(component.ImageLabel.enabled).toBeTrue();
  });

  it('should close dialog with image',  () =>
  {
    // component.config.isAvatar = false;
    // fixture.detectChanges();
    // fixture.whenStable().then( async () =>
    // {
    //   const close = fixture.debugElement.query(By.css('#close')).nativeElement;
    //   expect(close).toBeTruthy();
    //   close.click();
    //   tick();
    //   expect(dialogRefMock.close).toHaveBeenCalled();
    // });
  });
});
