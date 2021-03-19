import {ComponentFixture, TestBed} from '@angular/core/testing';

import {SearchComponent} from './search.component';
import {EventService} from "../_Services/event.service";
import {of, throwError} from "rxjs";
import {EventViewModel, SearchFilter, SearchParam} from "../_models/Event";
import {Category} from "../_models/Category";
import {HttpErrorResponse} from "@angular/common/http";
import {RouterTestingModule} from "@angular/router/testing";
import {Navigation, Router} from "@angular/router";


describe('SearchComponent', () =>
{
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;

  let eventServiceMock;

  beforeEach(async () =>
  {
    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllCategories', 'Search']);
    eventServiceMock.GetAllCategories.and.returnValue(of());
    eventServiceMock.Search.and.returnValue(of());

    await TestBed.configureTestingModule({
      declarations: [SearchComponent],
      imports: [RouterTestingModule],
      providers: [
        {provide: EventService, useValue: eventServiceMock},
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    let router = TestBed.inject(Router);
    let navigationSpy = spyOn(router,'getCurrentNavigation').and.returnValue({extras: {state: null}} as Navigation)
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  // it('should toggle current location', () =>
  // {
  //   const p: Position = {
  //     coords: {
  //       accuracy: 0,
  //       altitude: undefined,
  //       altitudeAccuracy: undefined,
  //       heading: undefined,
  //       latitude: 0,
  //       longitude: 1,
  //       speed: undefined
  //     }, timestamp: 0
  //   }
  //   // spyOn(navigator.geolocation, 'getCurrentPosition').and.callFake((position) => p);
  //   spyOn(navigator.geolocation,"getCurrentPosition").and.returnValue();
  //   component.toggleCurrentLocation(new MatSlideToggleChange(null, true));
  //   expect(component.usersLocation).toEqual(p);
  // });

  it('should search', () =>
  {
    const events: EventViewModel[] = [
      {
        Promos: [],
        Categories: [],
        Description: "",
        EndLocal: undefined,
        EndUTC: undefined,
        Id: "1",
        IsOnline: false,
        Name: "Test event",
        Price: 0,
        StartLocal: undefined,
        StartUTC: undefined,
        Thumbnail: undefined,
        Finished: false
      }
    ]
    eventServiceMock.Search.and.returnValue(of(events));
    component.search();
    expect(component.loading).toBeFalse();
    expect(component.events).toEqual(events);
  });

  it('should add category filters', () =>
  {
    component.selectedCategories = [
      {Id: "1", Name: "Music"},
      {Id: "2", Name: "Performance"}
    ];
    eventServiceMock.Search.and.returnValue(of(null));
    component.search();
    const filters: SearchFilter[] = [
      {Key: SearchParam.Category, Value: "1"},
      {Key: SearchParam.Category, Value: "2"}
    ];
    expect(eventServiceMock.Search).toHaveBeenCalledWith('', filters);
  });

  it('should add online filter', () =>
  {
    eventServiceMock.Search.and.returnValue(of(null));
    component.isOnline = true;
    component.search();
    const filters: SearchFilter[] = [
      {Key: SearchParam.IsOnline, Value: "true"}
    ];
    expect(eventServiceMock.Search).toHaveBeenCalledWith('', filters);
  });

  it('should add location filter', () =>
  {
    eventServiceMock.Search.and.returnValue(of(null));
    component.usingCurrentLocation = true;
    component.isOnline = false;
    component.usersLocation = {
      coords: {
        accuracy: 0,
        altitude: undefined,
        altitudeAccuracy: undefined,
        heading: undefined,
        latitude: 0,
        longitude: 1,
        speed: undefined
      }, timestamp: 0
    };
    component.distanceSelect = "1000";
    component.search();
    const filters: SearchFilter[] = [
      {Key: SearchParam.Location, Value: "0,1,1000"}
    ]
    expect(eventServiceMock.Search).toHaveBeenCalledWith('', filters);
  });

  it('should add date filter', () =>
  {
    eventServiceMock.Search.and.returnValue(of(null));
    component.date = new Date(0);
    component.usingDate = true;
    component.search();
    const filters: SearchFilter[] = [
      {Key: SearchParam.Date, Value: (new Date(0)).toDateString()}
    ];
    expect(eventServiceMock.Search).toHaveBeenCalledWith('', filters);
  });

  it('should get all categories', () =>
  {
    const categories: Category[] = [
      {Id: "1", Name: "Music"},
      {Id: "2", Name: "Performance"}
    ]
    eventServiceMock.GetAllCategories.and.returnValue(of(categories));
    component.ngOnInit();
    expect(component.categories).toEqual(categories);
  });

  it('should handle get all categories error', () =>
  {
    eventServiceMock.GetAllCategories.and.returnValue(throwError(new HttpErrorResponse({error: {Message: "Error getting categories"}})));
    component.ngOnInit();
    expect(component.getCategoriesError).toEqual("Error getting categories");
  });
});
