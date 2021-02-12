import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ImageViewModel} from "../_models/Image";
import {ImageUploadComponent, uploadConfig} from "../_extensions/image-upload/image-upload.component";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'thumbnail-edit',
  templateUrl: './thumbnail-edit.component.html',
  styleUrls: ['./thumbnail-edit.component.css']
})
export class ThumbnailEditComponent implements OnInit
{

  @Input() public thumbnail: ImageViewModel;
  @Output() public thumbnailEvent = new EventEmitter<ImageViewModel>();

  constructor (private dialog: MatDialog)
  {
  }

  ngOnInit (): void
  {
  }

  public thumbnailUpload ()
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
        this.thumbnail = image;
        this.thumbnailEvent.emit(this.thumbnail);
      }
    });
  }
}
