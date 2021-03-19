import {async, ComponentFixture, fakeAsync, TestBed, tick} from "@angular/core/testing";
import {Router} from "@angular/router";
import {PublicNavComponent} from "./public-nav.component";
import {RouterTestingModule} from "@angular/router/testing";
import {MatToolbarModule} from "@angular/material/toolbar";
import {Component} from "@angular/core";
import {By} from "@angular/platform-browser";
import {Location} from '@angular/common';

describe('PublicNavComponent', () =>
{
  let component: PublicNavComponent;
  let fixture: ComponentFixture<PublicNavComponent>;
  let router;
  let location;

  @Component({ template: '' })
  class DummyComponent {}

  beforeEach(async(() =>
  {
    TestBed.configureTestingModule({
      declarations: [PublicNavComponent,DummyComponent],
      imports: [
        RouterTestingModule.withRoutes([{path:'login',component: DummyComponent}]),
        MatToolbarModule,
      ],
    }).compileComponents();
  }));

  beforeEach(() =>
    {
      router = TestBed.inject(Router);
      location = TestBed.inject(Location);
      fixture = TestBed.createComponent(PublicNavComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }
  );

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should route home', () =>
  {
    let routerSpy = spyOn(router, 'navigate');
    component.routeHome();
    expect(routerSpy).toHaveBeenCalledWith(['/']);
  });

  it('should route to login when clicked', fakeAsync(() =>
  {
    let button = fixture.debugElement
      .query(By.css('#login'));

    expect(!!button).toBe(true);
    expect(button.nativeElement.classList.contains("activeLink")).toBeFalsy();

    button.nativeElement.click();

    tick();

    expect(button.nativeElement.classList.contains("activeLink"));
    expect(location.path()).toBe("/login");
  }));
});
