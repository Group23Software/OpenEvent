import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ImageCroppedEvent} from "ngx-image-cropper";
import {DialogRef} from "../../login/create-account/create-account.component.spec";
import {FormControl} from "@angular/forms";
import {ImageViewModel} from "../../_models/Image";

export interface uploadConfig
{
  height: number;
  width: number;
  isAvatar: boolean;
}


@Component({
  selector: 'image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})
export class ImageUploadComponent implements OnInit
{

  constructor (@Inject(MAT_DIALOG_DATA) public config: uploadConfig, private dialogRef: MatDialogRef<ImageUploadComponent>)
  {
  }

  ngOnInit (): void
  {
  }

  imageChangedEvent: any = '';
  croppedImage: any = '';
  ImageLabel = new FormControl({value: '', disabled: true});

  fileChangeEvent (event: any): void
  {
    this.ImageLabel.disable();
    this.imageChangedEvent = event;
  }

  imageCropped (event: ImageCroppedEvent)
  {
    this.croppedImage = event.base64;
  }

  imageLoaded (image: HTMLImageElement)
  {
    // show cropper
  }

  cropperReady ()
  {
    // cropper ready
    this.ImageLabel.enable();
  }

  loadImageFailed ()
  {
    // show message
  }

  close ()
  {
    if (this.config.isAvatar)
    {
      this.dialogRef.close(this.croppedImage as string);
    } else
    {
      this.dialogRef.close({
        Source: this.croppedImage,
        Label: this.ImageLabel.value
      } as ImageViewModel);
    }

  }
}
