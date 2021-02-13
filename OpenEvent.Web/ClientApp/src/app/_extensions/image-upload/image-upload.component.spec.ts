import {ComponentFixture, TestBed, tick} from '@angular/core/testing';

import {ImageUploadComponent} from './image-upload.component';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {By} from "@angular/platform-browser";
import {ImageCroppedEvent} from "ngx-image-cropper";
import {ImageViewModel} from "../../_models/Image";

describe('ImageUploadComponent', () =>
{
  let component: ImageUploadComponent;
  let fixture: ComponentFixture<ImageUploadComponent>;

  let dialogRefMock;

  beforeEach(async () =>
  {

    dialogRefMock = jasmine.createSpyObj('matDialogRef', ['close']);

    await TestBed.configureTestingModule({
      declarations: [ImageUploadComponent],
      providers: [
        {provide: MAT_DIALOG_DATA, useValue: {}},
        {provide: MatDialogRef, useValue: dialogRefMock},
      ]
    })
                 .compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(ImageUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should disable label when file changed', () =>
  {
    component.fileChangeEvent("Event");
    expect(component.imageChangedEvent).toEqual("Event");
    expect(component.ImageLabel.disabled).toBeTrue();
  });

  it('should enable label input when ready', () =>
  {
    component.cropperReady();
    expect(component.ImageLabel.enabled).toBeTrue();
  });

  it('should should crop image', () =>
  {
    component.imageCropped({base64: "imageBase64"} as ImageCroppedEvent);
    expect(component.croppedImage).toEqual("imageBase64");
  });

  it('should close dialog with avatar', () =>
  {
    component.config.isAvatar = true;

    fixture.detectChanges();
    component.close();

    expect(dialogRefMock.close).toHaveBeenCalled();
  });

  it('should close dialog with image', () =>
  {
    component.ImageLabel.setValue('Image Label');
    fixture.detectChanges();
    component.close();
    expect(dialogRefMock.close).toHaveBeenCalledWith({
      Source: component.croppedImage,
      Label: component.ImageLabel.value
    } as ImageViewModel);
  });
});
