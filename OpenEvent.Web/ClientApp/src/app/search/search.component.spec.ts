import {ComponentFixture, TestBed} from '@angular/core/testing';

import {SearchComponent} from './search.component';
import {EventService} from "../_Services/event.service";
import {of} from "rxjs";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('SearchComponent', () =>
{
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;

  let eventServiceMock;

  beforeEach(async () =>
  {

    eventServiceMock = jasmine.createSpyObj('eventService', ['GetAllCategories','Search']);
    eventServiceMock.GetAllCategories.and.returnValue(of());
    eventServiceMock.Search.and.returnValue(of());

    await TestBed.configureTestingModule({
      // imports: [HttpClientTestingModule],
      declarations: [SearchComponent],
      providers: [
        {provide: EventService, useValue: eventServiceMock}
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });
});
