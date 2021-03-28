import {ComponentFixture, TestBed} from '@angular/core/testing';

import {DemographicComponent} from './demographic.component';
import {ChartsModule} from "ng2-charts";

describe('DemographicComponent', () =>
{
  let component: DemographicComponent;
  let fixture: ComponentFixture<DemographicComponent>;

  beforeEach(async () =>
  {
    await TestBed.configureTestingModule({
      imports: [ChartsModule],
      declarations: [DemographicComponent],
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(DemographicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
