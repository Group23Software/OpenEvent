import {ComponentFixture, TestBed} from "@angular/core/testing";
import {ExploreComponent} from "./explore.component";
import {EventService} from "../_Services/event.service";
import {UserService} from "../_Services/user.service";
import {TriggerService} from "../_Services/trigger.service";
import {PopularityService} from "../_Services/popularity.service";
import {of} from "rxjs";
import {trigger} from "@angular/animations";
import {MatCardModule} from "@angular/material/card";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {Component, Input} from "@angular/core";

@Component({
  selector: 'popular-events',
  template:''
})
class MockPopularEvents {@Input() PopularEvents: any;}

describe('ExploreComponent', () =>
{
  let component: ExploreComponent;
  let fixture: ComponentFixture<ExploreComponent>;

  let eventServiceMock;
  let userServiceMock;
  let triggerServiceMock;
  let popularityServiceMock;

  beforeEach(async () =>
  {
    eventServiceMock = jasmine.createSpyObj('EventService',['GetAllCategories','Explore']);
    eventServiceMock.GetAllCategories.and.returnValue(of(null));
    eventServiceMock.Explore.and.returnValue(of(null));
    userServiceMock = jasmine.createSpyObj('UserService',['']);
    triggerServiceMock = jasmine.createSpyObj('TriggerService',['Iterate']);
    popularityServiceMock = jasmine.createSpyObj('PopularityService',['GetEvents','GetCategories']);
    popularityServiceMock.GetEvents.and.returnValue(of(null));
    popularityServiceMock.GetCategories.and.returnValue(of(null));

    await TestBed.configureTestingModule({
      declarations: [
        ExploreComponent,
        MockPopularEvents
      ],
      imports: [
        MatCardModule,
        MatProgressBarModule
      ],
      providers: [
        {provide: EventService, useValue: eventServiceMock},
        {provide: UserService, useValue: userServiceMock},
        {provide: TriggerService, useValue: triggerServiceMock},
        {provide: PopularityService, useValue: popularityServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(ExploreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
