import {PublicNavComponent} from "../public-nav/public-nav.component";
import {async, ComponentFixture, fakeAsync, TestBed, tick} from "@angular/core/testing";
import {Component, EventEmitter} from "@angular/core";
import {RouterTestingModule} from "@angular/router/testing";
import {MatToolbarModule} from "@angular/material/toolbar";
import {Router} from "@angular/router";
import {Location} from "@angular/common";
import {UserNavComponent} from "./user-nav.component";
import {MatMenuModule} from "@angular/material/menu";
import {MatIconModule} from "@angular/material/icon";
import {MatSlideToggleModule} from "@angular/material/slide-toggle";
import {themePreferenceBody, UserService} from "../../_Services/user.service";
import {TriggerService} from "../../_Services/trigger.service";
import {MatSnackBar, MatSnackBarModule} from "@angular/material/snack-bar";
import {UpdateThemePreferenceBody, UserAccountModel} from "../../_Models/User";
import {Observable, of} from "rxjs";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";

export class UserServiceStub
{
  set User (value: UserAccountModel)
  {
    this._User = value;
  }

  get User (): UserAccountModel
  {
    return this._User;
  }

  private _User: UserAccountModel = {Avatar: "", Id: "1", IsDarkMode: false, UserName: ""};

  public LogOut (): void
  {
  }

  public UpdateThemePreference (updateThemePreferenceBody: UpdateThemePreferenceBody): Observable<themePreferenceBody>
  {
    let body: themePreferenceBody = {
      isDarkMode: updateThemePreferenceBody.IsDarkMode
    }
    return of(body);
  }

}

class TriggerServiceStub
{
  public isDark: EventEmitter<boolean> = new EventEmitter<boolean>();
}

describe('UserNavComponent', () =>
{
  let component: UserNavComponent;
  let fixture: ComponentFixture<UserNavComponent>;
  let router;
  let location;
  let triggerServiceMock;
  let trigger;

  @Component({template: ''})
  class DummyComponent
  {
  }

  let userServiceMock;

  beforeEach(async(() =>
  {

    userServiceMock = jasmine.createSpyObj<UserService>('userService', ['UpdateThemePreference', 'User']);
    userServiceMock.User = {
      get: m => m.returnValue(null),
      set: null
    }

    triggerServiceMock = jasmine.createSpyObj<TriggerService>('triggerService',['isDark']);

    TestBed.configureTestingModule({
      declarations: [UserNavComponent, DummyComponent],
      imports: [
        RouterTestingModule,
        MatToolbarModule,
        MatMenuModule,
        MatIconModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        BrowserAnimationsModule
      ],
      providers: [
        // {provide: UserService, userValue: userServiceMock},
        {provide: UserService, useClass: UserServiceStub},
        {provide: TriggerService, useClass: TriggerServiceStub},
        MatSnackBar
      ]
    }).compileComponents();
  }));

  beforeEach(() =>
    {
      router = TestBed.get(Router);
      location = TestBed.get(Location);
      trigger = TestBed.get(TriggerService);
      fixture = TestBed.createComponent(UserNavComponent);
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


  it('should logout', () =>
  {
    let routerSpy = spyOn(router, 'navigate');
    component.logout();
    expect(routerSpy).toHaveBeenCalledWith(['/login']);
  });

  // it('should toggle theme', fakeAsync(() =>
  // {
  //   let triggerSpy = spyOn(trigger.isDark,'emit');
  //   // let userServiceSpy = spyOn(TestBed.get(UserService), 'UpdateThemePreference');
  //   component.toggleTheme(true);
  //   tick();
  //   fixture.detectChanges();
  //
  //   expect(triggerSpy).toHaveBeenCalled();
  //   // expect(userServiceSpy).toHaveBeenCalled();
  // }));

});
