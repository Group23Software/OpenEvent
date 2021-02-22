import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDataComponent } from './user-data.component';
import {UserService} from "../../_Services/user.service";
import {of} from "rxjs";

describe('UserDataComponent', () => {
  let component: UserDataComponent;
  let fixture: ComponentFixture<UserDataComponent>;

  let userServiceMock;

  beforeEach(async () => {

    userServiceMock = jasmine.createSpyObj('UserService',['GetAnalytics']);
    userServiceMock.GetAnalytics.and.returnValue(of(true));

    await TestBed.configureTestingModule({
      declarations: [ UserDataComponent ],
      providers: [
        {provide: UserService, useValue: userServiceMock}
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
