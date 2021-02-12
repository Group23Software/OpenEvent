import {ComponentFixture, TestBed, tick} from '@angular/core/testing';

import {ThumbnailEditComponent} from './thumbnail-edit.component';
import {ImageListComponent} from "../image-list/image-list.component";
import {MatDialog} from "@angular/material/dialog";
import {By} from "@angular/platform-browser";
import {ImageUploadComponent, uploadConfig} from "../_extensions/image-upload/image-upload.component";

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
    fixture.detectChanges();
    fixture.whenStable().then(() =>
    {
      const newButton = fixture.debugElement.query(By.css('#newThumbnailButton'));
      newButton.nativeElement.click();
      expect(dialogMock.open).toHaveBeenCalledWith(ImageUploadComponent, {
        data: {
          height: 3,
          width: 4
        } as uploadConfig
      });
    });
  });

  it('should emit thumbnail', async () =>
  {
    fixture.detectChanges();
    fixture.whenStable().then(async () =>
    {
      const newButton = fixture.debugElement.query(By.css('#newThumbnailButton'));
      newButton.nativeElement.click();
      tick();
      expect(component.thumbnailEvent.emit).toHaveBeenCalledWith(component.thumbnail);
    });
  });
});
