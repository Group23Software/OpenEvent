import {async, ComponentFixture, fakeAsync, TestBed, waitForAsync} from '@angular/core/testing';
import {EventConfigComponent} from './event-config.component';
import {RouterTestingModule} from "@angular/router/testing";
import {EventService} from "../../_Services/event.service";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar} from "@angular/material/snack-bar";
import {of} from "rxjs";
import {ImageUploadComponent, uploadConfig} from "../../_extensions/image-upload/image-upload.component";
import {By} from "@angular/platform-browser";
import {HarnessLoader} from "@angular/cdk/testing";
import {TestbedHarnessEnvironment} from "@angular/cdk/testing/testbed";
import {MatCheckboxHarness} from "@angular/material/checkbox/testing";
import {MatCardModule} from "@angular/material/card";
import {MatCheckbox} from "@angular/material/checkbox";

describe('EventConfigComponent', () =>
{
  let component: EventConfigComponent;
  let fixture: ComponentFixture<EventConfigComponent>;
  let loader: HarnessLoader;
  let rootLoader: HarnessLoader;

  let snackBarMock;
  let dialogMock;
  let eventServiceMock;

  beforeEach(async () =>
  {
    snackBarMock = jasmine.createSpyObj('matSnackBar', ['open']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllCategories', 'GetForHost', 'HostsEvents']);
    eventServiceMock.GetAllCategories.and.returnValue(of());
    eventServiceMock.GetForHost.and.returnValue(of());

    await TestBed.configureTestingModule({
      declarations: [EventConfigComponent],
      imports: [
        RouterTestingModule.withRoutes([]),
        MatCardModule
      ],
      providers: [
        {provide: EventService, useValue: eventServiceMock},
        {provide: MatDialog, useValue: dialogMock},
        {provide: MatSnackBar, useValue: snackBarMock},
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(EventConfigComponent);
    component = fixture.componentInstance;
    component.event = {
      Address: undefined,
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Id: "",
      Images: [],
      IsOnline: false,
      Name: "",
      Price: 0,
      SocialLinks: [],
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined,
      Tickets: [],
      TicketsLeft: 0
    }
    fixture.detectChanges();
    loader = TestbedHarnessEnvironment.loader(fixture);
    rootLoader = TestbedHarnessEnvironment.documentRootLoader(fixture);
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should load form data', () =>
  {

  });

  it('should add category', () =>
  {
    component.categoryStore = [
      {Name: "Music", Id: "1"},
      {Name: "Comedy", Id: "2"}
    ];
    component.addCategory({Name: "Music", Id: "1"});
    expect(component.categories).toEqual([{Name: "Music", Id: "1"}]);
    expect(component.categoryStore).toEqual([{Name: "Comedy", Id: "2"}]);
  });

  it('should remove category', () =>
  {
    component.categories = [
      {Name: "Music", Id: "1"},
      {Name: "Comedy", Id: "2"}
    ];
    component.removeCategory({Name: "Music", Id: "1"});
    expect(component.categories).toEqual([{Name: "Comedy", Id: "2"}]);
    expect(component.categoryStore).toEqual([{Name: "Music", Id: "1"}]);
  });

  it('should disable address from', fakeAsync(() =>
  {
    fixture.detectChanges();
    fixture.whenStable().then(() =>
    {
      let isOnline = fixture.debugElement.query(By.css('#isOnline'));
      isOnline.triggerEventHandler("click",{});
      fixture.detectChanges();
      for (let control in component.addressForm.controls)
      {
        expect(component.addressForm.controls[control].disabled).toBeTrue();
      }
    });
  }));

  // TODO: fix this test (harness?)
  // it('should re-enable address form', () =>
  // {
  //   fixture.detectChanges();
  //   fixture.whenStable().then(() =>
  //   {
  //     let isOnline = fixture.debugElement.query(By.css('#isOnline'));
  //     isOnline.triggerEventHandler("click",{});
  //     isOnline.triggerEventHandler("click",{});
  //     fixture.detectChanges();
  //     for (let control in component.addressForm.controls)
  //     {
  //       expect(component.addressForm.controls[control].enabled).toBeTrue();
  //     }
  //   });
  // });

  it('should update event', () =>
  {

  });

  it('should open image upload', () =>
  {
    fixture.detectChanges();
    fixture.whenStable().then(() =>
    {
      const newButton = fixture.debugElement.query(By.css('#newImageButton'));
      newButton.nativeElement.click();
      expect(dialogMock.open).toHaveBeenCalledWith(ImageUploadComponent, {
        data: {
          height: 3,
          width: 4
        } as uploadConfig
      });
    });
  });

  it('should upload image', () =>
  {

  });

  it('should upload thumbnail', () =>
  {

  });

  it('should remove image', () =>
  {

  });
});
