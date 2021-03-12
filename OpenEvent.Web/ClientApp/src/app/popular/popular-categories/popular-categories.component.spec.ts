import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopularCategoriesComponent } from './popular-categories.component';
import {RouterTestingModule} from "@angular/router/testing";

describe('PopularCategoriesComponent', () => {
  let component: PopularCategoriesComponent;
  let fixture: ComponentFixture<PopularCategoriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PopularCategoriesComponent ],
      imports: [RouterTestingModule]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PopularCategoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
