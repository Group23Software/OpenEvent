import {ComponentFixture, TestBed} from '@angular/core/testing';

import {PopularCategoriesComponent} from './popular-categories.component';
import {RouterTestingModule} from "@angular/router/testing";
import {CategoryViewModel} from "../../_models/Category";
import {Router} from "@angular/router";
import {SearchFilter, SearchParam} from "../../_models/Event";
import {MatCardModule} from "@angular/material/card";
import {MatIconModule} from "@angular/material/icon";

describe('PopularCategoriesComponent', () =>
{
  let component: PopularCategoriesComponent;
  let fixture: ComponentFixture<PopularCategoriesComponent>;
  let router;

  beforeEach(async () =>
  {
    await TestBed.configureTestingModule({
      declarations: [PopularCategoriesComponent],
      imports: [
        RouterTestingModule,
        MatCardModule,
        MatIconModule
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(PopularCategoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should navigate to category', () =>
  {
    let routerSpy = spyOn(router,'navigateByUrl');
    let category: CategoryViewModel = {Id: "0", Name: "Music"};
    component.navigateToCategory(category);
    expect(routerSpy).toHaveBeenCalledWith('/search', {
      state: {
        keyword: "",
        filters: [{Key: SearchParam.Category, Value: category.Id}] as SearchFilter[]
      }
    })
  });
});
