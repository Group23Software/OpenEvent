import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ImageListComponent} from './image-list.component';
import {MatDialog} from "@angular/material/dialog";
import {By} from "@angular/platform-browser";
import {ImageUploadComponent, uploadConfig} from "../_extensions/image-upload/image-upload.component";
import {of} from "rxjs";

describe('ImageListComponent', () =>
{
  let component: ImageListComponent;
  let fixture: ComponentFixture<ImageListComponent>;

  let dialogMock;

  beforeEach(async () =>
  {

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);
    dialogMock.open.and.returnValue({afterClosed: () => of(true)});

    await TestBed.configureTestingModule({
      declarations: [ImageListComponent],
      providers: [
        {provide: MatDialog, useValue: dialogMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(ImageListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should open dialog', () =>
  {
    component.imageUpload();
    expect(dialogMock.open).toHaveBeenCalledWith(ImageUploadComponent, {
      data: {
        height: 3,
        width: 4
      } as uploadConfig
    });
  });

  it('should emit image array', () =>
  {
    spyOn(component.imageEvent, 'emit');
    component.images = [{Label: "label", Source: "Source"}];
    component.imageUpload();
    expect(component.imageEvent.emit).toHaveBeenCalledWith(component.images);

  });

  it('should remove image',  () =>
  {
    component.images = [{Label: "label", Source: "Source"},{Label: "secondLabel", Source: "secondSource"}];
    component.removeImage(component.images[0]);
    expect(component.images).toEqual([{Label: "secondLabel", Source: "secondSource"}]);
  });
});
