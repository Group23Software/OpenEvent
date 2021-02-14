import {ComponentFixture, TestBed, tick} from '@angular/core/testing';

import {ThumbnailEditComponent} from './thumbnail-edit.component';
import {ImageListComponent} from "../image-list/image-list.component";
import {MatDialog} from "@angular/material/dialog";
import {By} from "@angular/platform-browser";
import {ImageUploadComponent, uploadConfig} from "../_extensions/image-upload/image-upload.component";
import {of} from "rxjs";
import {ImageViewModel} from "../_models/Image";

describe('ThumbnailEditComponent', () =>
{
  let component: ThumbnailEditComponent;
  let fixture: ComponentFixture<ThumbnailEditComponent>;

  let dialogMock;

  beforeEach(async () =>
  {

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    await TestBed.configureTestingModule({
      declarations: [ThumbnailEditComponent],
      providers: [{provide: MatDialog, useValue: dialogMock}]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(ThumbnailEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should open dialog', () =>
  {
    dialogMock.open.and.returnValue({afterClosed: () => of(null)});
    component.thumbnailUpload();
    expect(dialogMock.open).toHaveBeenCalledWith(ImageUploadComponent, {
      data: {
        height: 3,
        width: 4
      } as uploadConfig
    });
  });

  it('should emit thumbnail', async () =>
  {
    spyOn(component.thumbnailEvent, 'emit');
    dialogMock.open.and.returnValue({afterClosed: () => of({Source: "Source", Label: "Label"} as ImageViewModel)})
    component.thumbnailUpload();
    expect(component.thumbnail).toEqual({Source: "Source", Label: "Label"});
    expect(component.thumbnailEvent.emit).toHaveBeenCalledWith(component.thumbnail);
  });
});
