import {Component, Input, OnInit, Output} from '@angular/core';
import {ImageViewModel} from "../_models/Image";
import {EventEmitter} from "@angular/core";
import {ImageUploadComponent, uploadConfig} from "../_extensions/image-upload/image-upload.component";
import {MatDialog} from "@angular/material/dialog";
import {ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR} from "@angular/forms";

@Component({
  selector: 'image-list',
  templateUrl: './image-list.component.html',
  styleUrls: ['./image-list.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ImageListComponent
    },
    {
      provide: NG_VALIDATORS,
      useExisting: ImageListComponent,
      multi: true
    }
  ]
})
export class ImageListComponent implements ControlValueAccessor, OnInit
{

  @Input() images: ImageViewModel[] = [];
  @Output() public imageEvent = new EventEmitter<ImageViewModel[]>();

  onChange = (images) =>
  {
  };

  onTouched = () =>
  {
  };

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
        this.images.push(image);
        this.change();
        this.imageEvent.emit(this.images);
      }
    });
  }

  private change() {
    this.onChange(this.images);
    this.onTouched();
  }

  public removeImage (image: ImageViewModel)
  {
    this.images = this.images.filter(x => x != image);
    this.change();
  }

  registerOnChange (fn: any): void
  {
    this.onChange = fn;
  }

  registerOnTouched (fn: any): void
  {
    this.onTouched = fn;
  }

  writeValue (images: ImageViewModel[]): void
  {
    if (images != null) this.images = images;
  }
}
