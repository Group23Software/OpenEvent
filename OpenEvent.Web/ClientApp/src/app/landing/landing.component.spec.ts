import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LandingComponent } from './landing.component';
import {RouterTestingModule} from "@angular/router/testing";
import {PopularityService} from "../_Services/popularity.service";

describe('LandingComponent', () => {
  let component: LandingComponent;
  let fixture: ComponentFixture<LandingComponent>;

  let popularityServiceMock;

  beforeEach(async () => {

    popularityServiceMock = jasmine.createSpyObj('PopularityService',['ListenToEvents','ListenToCategories','GetEvents','GetCategories'])

    await TestBed.configureTestingModule({
      declarations: [ LandingComponent ],
      imports: [RouterTestingModule],
      providers: [
        {provide: PopularityService, useValue: popularityServiceMock}
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
