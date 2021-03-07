import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IteratorComponent } from './iterator.component';
import {AnimationBuilder} from "@angular/animations";

describe('IteratorComponent', () => {
  let component: IteratorComponent;
  let fixture: ComponentFixture<IteratorComponent>;

  let animationBuilderMock;

  beforeEach(async () => {

    animationBuilderMock = jasmine.createSpyObj('AnimationBuilder',['build'])

    await TestBed.configureTestingModule({
      declarations: [ IteratorComponent ],
      providers: [
        {provide: AnimationBuilder, useValue: animationBuilderMock}
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IteratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
