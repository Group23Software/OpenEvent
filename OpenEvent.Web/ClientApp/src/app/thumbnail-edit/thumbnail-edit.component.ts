import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ImageViewModel} from "../_models/Image";
import {ImageUploadComponent, uploadConfig} from "../_extensions/image-upload/image-upload.component";
import {MatDialog} from "@angular/material/dialog";
import {ControlValueAccessor, FormControl, NG_VALIDATORS, NG_VALUE_ACCESSOR} from "@angular/forms";

@Component({
  selector: 'thumbnail-edit',
  templateUrl: './thumbnail-edit.component.html',
  styleUrls: ['./thumbnail-edit.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: ThumbnailEditComponent
    },
    {
      provide: NG_VALIDATORS,
      useExisting: ThumbnailEditComponent,
      multi: true
    }
  ]
})
export class ThumbnailEditComponent implements ControlValueAccessor, OnInit
{

  @Input() public thumbnail: ImageViewModel;
  @Output() public thumbnailEvent = new EventEmitter<ImageViewModel>();

  onChange = (thumbnail) =>
  {
  };

  onTouched = () =>
  {
  };

  constructor (private dialog: MatDialog)
  {
  }

  @Input() control: FormControl;

  ngOnInit (): void
  {
  }

  public thumbnailUpload ()
  {
    this.onTouched();

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
        this.onChange(this.thumbnail);
        this.onTouched();

        this.thumbnailEvent.emit(this.thumbnail);
      }
    });
  }

  registerOnChange (fn: any): void
  {
    this.onChange = fn;
  }

  registerOnTouched (fn: any): void
  {
    this.onTouched = fn;
  }

  writeValue (thumbnail: ImageViewModel): void
  {
    this.onTouched();
    if (thumbnail != null) this.thumbnail = thumbnail;
  }

  validate(control: FormControl)
  {
    if (control.touched)
    {
      if (control.value == null) return {required: true}
    }
    return false;
  }
}
