import {async, ComponentFixture, fakeAsync, TestBed, waitForAsync} from '@angular/core/testing';
import {EventConfigComponent} from './event-config.component';
import {RouterTestingModule} from "@angular/router/testing";
import {EventService} from "../../_Services/event.service";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Observable, of} from "rxjs";
import {ImageUploadComponent, uploadConfig} from "../../_extensions/image-upload/image-upload.component";
import {By} from "@angular/platform-browser";
import {HarnessLoader} from "@angular/cdk/testing";
import {TestbedHarnessEnvironment} from "@angular/cdk/testing/testbed";
import {MatCheckboxHarness} from "@angular/material/checkbox/testing";
import {MatCardModule} from "@angular/material/card";
import {MatCheckbox} from "@angular/material/checkbox";
import {MatButtonHarness} from "@angular/material/button/testing";
import {EventHostModel} from "../../_models/Event";
import {Category} from "../../_models/Category";
import {ActivatedRoute} from "@angular/router";

describe('EventConfigComponent', () =>
{
  let component: EventConfigComponent;
  let fixture: ComponentFixture<EventConfigComponent>;
  let loader: HarnessLoader;

  let snackBarMock;
  let dialogMock;
  let eventServiceMock;

  beforeEach(async () =>
  {
    snackBarMock = jasmine.createSpyObj('matSnackBar', ['open']);

    dialogMock = jasmine.createSpyObj('matDialog', ['open']);

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllCategories', 'GetForHost'], ['HostsEvents']);
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
        {provide: ActivatedRoute, useValue: {params: of({id: 'test'})}}
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
    loader = TestbedHarnessEnvironment.loader(fixture);
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should load form data', () =>
  {
    // spyOnProperty(eventServiceMock,'HostsEvents').and.returnValue([{
    //   Address: undefined,
    //   Categories: [],
    //   Description: "",
    //   EndLocal: undefined,
    //   EndUTC: undefined,
    //   Id: "test",
    //   Images: [],
    //   IsOnline: false,
    //   Name: "",
    //   Price: 0,
    //   SocialLinks: [],
    //   StartLocal: undefined,
    //   StartUTC: undefined,
    //   Thumbnail: undefined,
    //   Tickets: [],
    //   TicketsLeft: 0
    // }] as EventHostModel[]);

    console.log(eventServiceMock);

    // Object.getOwnPropertyDescriptor(eventServiceMock, "x").get.and.returnValue(7);

    // eventServiceMock.HostsEvents.get.and.returnValue([{
    //   Address: undefined,
    //   Categories: [],
    //   Description: "",
    //   EndLocal: undefined,
    //   EndUTC: undefined,
    //   Id: "test",
    //   Images: [],
    //   IsOnline: false,
    //   Name: "",
    //   Price: 0,
    //   SocialLinks: [],
    //   StartLocal: undefined,
    //   StartUTC: undefined,
    //   Thumbnail: undefined,
    //   Tickets: [],
    //   TicketsLeft: 0
    // }] as EventHostModel[]);
    // eventServiceMock.GetAllCategories.and.returnValue(of([{}] as Category[]));
    //
    // component.ngOnInit();


  });

  it('should disable address from', fakeAsync(() =>
  {
    fixture.detectChanges();
    fixture.whenStable().then(() =>
    {
      let isOnline = fixture.debugElement.query(By.css('#isOnline'));
      isOnline.triggerEventHandler("click", {});
      fixture.detectChanges();
      for (let control in component.addressForm.controls)
      {
        expect(component.addressForm.controls[control].disabled).toBeTrue();
      }
    });
  }));

  // TODO: fix this test (harness?)
  // it('should re-enable address form', async () =>
  // {
  //   fixture.detectChanges();
  //   fixture.whenStable().then(() =>
  //   {
  //     let isOnline = fixture.debugElement.query(By.css('#isOnline'));
  //     isOnline.triggerEventHandler("click", {});
  //     fixture.detectChanges();
  //     fixture.whenStable().then(() =>
  //     {
  //       isOnline.triggerEventHandler("click", {});
  //       fixture.detectChanges();
  //       for (let control in component.addressForm.controls)
  //       {
  //         expect(component.addressForm.controls[control].enabled).toBeTrue();
  //       }
  //     });
  //   });
  // });

  it('should update event', () =>
  {

  });
});
