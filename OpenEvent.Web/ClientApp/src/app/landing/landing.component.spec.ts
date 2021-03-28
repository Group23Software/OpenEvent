import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';

import {LandingComponent} from './landing.component';
import {RouterTestingModule} from "@angular/router/testing";
import {PopularityService} from "../_Services/popularity.service";
import {Router} from "@angular/router";
import {PopularEventViewModel} from "../_models/Event";
import {of} from "rxjs";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";

describe('LandingComponent', () =>
{
  let component: LandingComponent;
  let fixture: ComponentFixture<LandingComponent>;
  let router;

  let popularityServiceMock;

  beforeEach(async () =>
  {

    popularityServiceMock = jasmine.createSpyObj('PopularityService', ['ListenToEvents', 'ListenToCategories', 'GetEvents', 'GetCategories'])
    popularityServiceMock.GetEvents.and.returnValue(of(null));
    popularityServiceMock.GetCategories.and.returnValue(of(null));

    await TestBed.configureTestingModule({
      declarations: [LandingComponent],
      imports: [RouterTestingModule],
      providers: [
        {provide: PopularityService, useValue: popularityServiceMock}
      ]
    })
                 .compileComponents();
  });

  beforeEach(() =>
  {
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(LandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should navigate to featured', () =>
  {
    let event: PopularEventViewModel = {
      Categories: [],
      Description: "",
      EndLocal: undefined,
      EndUTC: undefined,
      Id: "EventId",
      IsOnline: false,
      Name: "",
      Price: 0,
      Promos: [],
      Score: 0,
      StartLocal: undefined,
      StartUTC: undefined,
      Thumbnail: undefined,
      Finished: false
    }
    let routeSpy = spyOn(router, 'navigate');
    spyOnProperty(component, 'FeaturedEvent', 'get').and.returnValue(event);
    component.navigateToFeatured();
    expect(routeSpy).toHaveBeenCalledWith(['/event', event.Id]);
  });

  it('should get events and categories', fakeAsync(() =>
  {
    component.ngOnInit();
    tick();
    expect(component.Loading).toBeFalse();
  }));

  // it('should handle get error', () =>
  // {
  //   // let err = new HttpErrorResponse(null);
  //   // err.message = 'Error getting data';
  //   popularityServiceMock.GetEvents.and.throwError(new HttpErrorResponse({error: {message: ''}}));
  //   popularityServiceMock.GetCategories.and.throwError(new HttpErrorResponse({error: {message: ''}}));
  //   component.ngOnInit();
  //   expect(component.Loading).toBeFalse();
  //   expect(component.Error).toEqual('Error getting data');
  // });
});
