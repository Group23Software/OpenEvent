import { ComponentFixture, TestBed } from '@angular/core/testing';
import {AccountComponent} from "./account.component";
import {UserService} from "../_Services/user.service";
import {of} from "rxjs";
import {MatDialog} from "@angular/material/dialog";


describe('AccountComponent', () => {
  let component: AccountComponent;
  let fixture: ComponentFixture<AccountComponent>;

  let userServiceMock;


  beforeEach(async () => {

    userServiceMock = jasmine.createSpyObj('userService', ['GetAccountUser','User']);
    userServiceMock.GetAccountUser.and.returnValue(of());

    await TestBed.configureTestingModule({
      imports: [],
      declarations: [ AccountComponent ],
      providers: [
        {provide: UserService, useValue: userServiceMock},

      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
