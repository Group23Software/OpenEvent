import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';

import {CategoryListComponent} from './category-list.component';
import {TestbedHarnessEnvironment} from "@angular/cdk/testing/testbed";
import {HarnessLoader} from "@angular/cdk/testing";
import {By} from "@angular/platform-browser";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatChipsModule} from "@angular/material/chips";
import {MatIconModule} from "@angular/material/icon";
import {MatDividerModule} from "@angular/material/divider";

describe('CategoryListComponent', () =>
{
  let component: CategoryListComponent;
  let fixture: ComponentFixture<CategoryListComponent>;
  let loader: HarnessLoader;

  beforeEach(async () =>
  {
    await TestBed.configureTestingModule({
      declarations: [CategoryListComponent],
      imports: [
        BrowserAnimationsModule,
        MatChipsModule,
        MatIconModule,
        MatDividerModule
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(CategoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    loader = TestbedHarnessEnvironment.loader(fixture);
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  // it('should add category to list', () =>
  // {
  //   component.categories = [
  //     {Id: "1", Name: "Music"},
  //     {Id: "2", Name: "Drama"}
  //   ];
  //   fixture.detectChanges();
  //   fixture.whenStable().then(async () =>
  //   {
  //     const icon = fixture.debugElement.query(By.css('#add')).nativeElement;
  //     expect(icon).toBeTruthy();
  //     icon.click();
  //     expect(component.categories.length).toBe(1);
  //     expect(component.selectedCategories.length).toBe(1);
  //     // expect(component.categoryEvent.emit).toHaveBeenCalled();
  //   })
  // });
  //
  // it('should remove category to list', fakeAsync(() =>
  // {
  //   component.selectedCategories = [
  //     {Id: "1", Name: "Music"},
  //     {Id: "2", Name: "Drama"}
  //   ];
  //   fixture.detectChanges();
  //   fixture.whenStable().then(() =>
  //   {
  //     const icon = fixture.debugElement.query(By.css('#remove')).nativeElement;
  //     expect(icon).toBeTruthy();
  //     icon.click();
  //     tick();
  //     expect(component.categories.length).toBe(1);
  //     expect(component.selectedCategories.length).toBe(1);
  //     // expect(component.categoryEvent.emit).toHaveBeenCalledWith(component.selectedCategories);
  //   });
  // }));
  //
  // it('should render chips', () =>
  // {
  //   component.categories = [
  //     {Id: "1", Name: "Music"},
  //     {Id: "2", Name: "Drama"}
  //   ];
  //   fixture.detectChanges();
  //   fixture.whenStable().then(async () =>
  //   {
  //     const chips = fixture.debugElement.queryAll(By.css('mat-chip'));
  //     expect(chips.length).toBe(2);
  //   });
  // });
});
