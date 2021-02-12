import {Component, Input, OnInit, Output} from '@angular/core';
import {ImageViewModel} from "../_models/Image";
import {EventEmitter} from "@angular/core";
import {ImageUploadComponent, uploadConfig} from "../_extensions/image-upload/image-upload.component";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'image-list',
  templateUrl: './image-list.component.html',
  styleUrls: ['./image-list.component.css']
})
export class ImageListComponent implements OnInit
{

  @Input() images: ImageViewModel[];
  @Output() public imageEvent = new EventEmitter<ImageViewModel[]>();

  constructor (private dialog: MatDialog)
  {
  }

  ngOnInit (): void
  {
  }

  public imageUpload ()
  {
    let ref = this.dialog.open(ImageUploadComponent, {
      data: {
        height: 3,
        width: 4
      } as uploadConfig
    });

    ref.afterClosed().subscribe((image: ImageViewModel) =>
    {
      if (image)
      {
        console.log(image);
        this.images.push(image);
        this.imageEvent.emit(this.images);
      }
    });
  }

  public removeImage (image: ImageViewModel)
  {
    this.images = this.images.filter(x => x != image);
  }
}
