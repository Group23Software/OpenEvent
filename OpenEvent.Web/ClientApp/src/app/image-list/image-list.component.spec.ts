import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ImageListComponent} from './image-list.component';
import {MatDialog} from "@angular/material/dialog";
import {By} from "@angular/platform-browser";
import {ImageUploadComponent, uploadConfig} from "../_extensions/image-upload/image-upload.component";

describe('ImageListComponent', () =>
{
  let component: ImageListComponent;
  let fixture: ComponentFixture<ImageListComponent>;

  let dialogMock;

  beforeEach(async () =>
  {

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    await TestBed.configureTestingModule({
      declarations: [ImageListComponent],
      providers: [{provide: MatDialog, useValue: dialogMock}]
    })
                 .compileComponents();
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
    fixture.detectChanges();
    fixture.whenStable().then(() =>
    {
      const newButton = fixture.debugElement.query(By.css('.imageDeleteButton'));
      newButton.nativeElement.click();
      expect(dialogMock.open).toHaveBeenCalledWith(ImageUploadComponent, {
        data: {
          height: 3,
          width: 4
        } as uploadConfig
      });
    });
  });

  it('should emit image array', () =>
  {
    component.images = [{Label: "label", Source: "Source"}];
    fixture.detectChanges();
    fixture.whenStable().then(() =>
    {
      const newButton = fixture.debugElement.query(By.css('.imageDeleteButton'));
      newButton.nativeElement.click();
      expect(component.imageEvent.emit).toHaveBeenCalledWith(component.images);
    });
  });
});
