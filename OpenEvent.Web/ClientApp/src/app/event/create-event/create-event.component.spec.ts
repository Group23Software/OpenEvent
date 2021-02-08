import {async, ComponentFixture, TestBed} from '@angular/core/testing';

import {CreateEventComponent} from './create-event.component';
import {MatDialog, MatDialogModule} from "@angular/material/dialog";
import {UserService} from "../../_Services/user.service";
import {EventService} from "../../_Services/event.service";
import {BrowserDynamicTestingModule} from "@angular/platform-browser-dynamic/testing";
import {of} from "rxjs";
import {ImageUploadComponent, uploadConfig} from "../../_extensions/image-upload/image-upload.component";

describe('CreateEventComponent', () =>
{
  let component: CreateEventComponent;
  let fixture: ComponentFixture<CreateEventComponent>;
  let dialogMock;
  let userServiceMock;
  let eventServiceMock;

  let dialogSpy: jasmine.Spy;
  let dialogRefSpyObj = jasmine.createSpyObj({afterClosed: of({}), close: null});

  beforeEach(async(() =>
  {

    dialogMock = jasmine.createSpyObj('matDialog', ['open', 'afterClosed']);

    userServiceMock = jasmine.createSpyObj('UserService', ['User']);
    eventServiceMock = jasmine.createSpyObj('EventService', ['GetAllCategories', 'Create']);
    eventServiceMock.GetAllCategories.and.returnValue(of());
    eventServiceMock.Create.and.returnValue(of());

    TestBed.configureTestingModule({
      declarations: [CreateEventComponent],
      imports: [
        BrowserDynamicTestingModule,
        MatDialogModule
      ],
      providers: [
        // {provide: MatDialog, useValue: dialogMock},
        {provide: UserService, useValue: userServiceMock},
        {provide: EventService, useValue: eventServiceMock}
      ]
    }).compileComponents();
  }));

  beforeEach(() =>
  {
    dialogSpy = spyOn(TestBed.get(MatDialog), 'open').and.returnValue(dialogRefSpyObj);
    fixture = TestBed.createComponent(CreateEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  //TODO: form control validation tests

  it('should disable / enable address form if online', () =>
  {
    component.IsOnline.setValue(false);
    fixture.detectChanges();
    component.clickedOnline();
    for (let control in component.addressForm.controls)
    {
      expect(component.addressForm.controls[control].disabled).toBeTruthy();
    }
    component.IsOnline.setValue(true);
    fixture.detectChanges();
    component.clickedOnline();
    for (let control in component.addressForm.controls)
    {
      expect(component.addressForm.controls[control].enabled).toBeTruthy();
    }
  });

  it('should open image upload dialog', () =>
  {
    component.imageUpload();
    expect(dialogSpy).toHaveBeenCalledWith(ImageUploadComponent, {
      data: {
        height: 3,
        width: 4
      } as uploadConfig
    });
    expect(dialogRefSpyObj.afterClosed).toHaveBeenCalled();
  });

  it('should open thumbnail upload dialog', () =>
  {
    component.thumbnailUpload();
    expect(dialogSpy).toHaveBeenCalledWith(ImageUploadComponent, {
      data: {
        height: 3,
        width: 4
      } as uploadConfig
    });
    expect(dialogRefSpyObj.afterClosed).toHaveBeenCalled();
  });

  it('should add category to list', () => {
    component.categoryStore = [{Id: "1", Name: "Music"},{Id: "2", Name: "Drama"}];
    component.addCategory({Id: "1", Name: "Music"});
    expect(component.categoryStore).toEqual([{Id: "2", Name: "Drama"}]);
    expect(component.categories).toEqual([{Id: "1", Name: "Music"}]);
  });

  it('should remove category to list', () => {
    component.categories = [{Id: "1", Name: "Music"},{Id: "2", Name: "Drama"}];
    component.removeCategory({Id: "1", Name: "Music"});
    expect(component.categoryStore).toEqual([{Id: "1", Name: "Music"}]);
    expect(component.categories).toEqual([{Id: "2", Name: "Drama"}]);
  });
});
