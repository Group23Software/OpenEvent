import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';

import {IteratorComponent} from './iterator.component';
import {AnimationBuilder, AnimationFactory} from "@angular/animations";
import {BrowserTestingModule} from "@angular/platform-browser/testing";
import {MockAnimationPlayer} from "@angular/animations/browser/testing";
import {NoopAnimationsModule} from "@angular/platform-browser/animations";

describe('IteratorComponent', () =>
{
  let component: IteratorComponent;
  let fixture: ComponentFixture<IteratorComponent>;

  let animationBuilderMock;

  beforeEach(async () =>
  {

    // let animationFactoryMock = jasmine.createSpyObj('AnimationFactory',['create']);

    // animationBuilderMock = jasmine.createSpyObj('AnimationBuilder', ['build']);
    // animationBuilderMock.build.and.returnValue(new AnimationFactory());

    await TestBed.configureTestingModule({
      declarations: [IteratorComponent],
      imports: [
        BrowserTestingModule,
        NoopAnimationsModule
      ],
      providers: [
        // {provide: AnimationBuilder, useValue: animationBuilderMock}
        NoopAnimationsModule
      ]
    }).compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(IteratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should close', fakeAsync(() =>
  {
    let destroySpy = spyOn(component, 'ngOnDestroy');
    component.close();
    tick(200);
    expect(destroySpy).toHaveBeenCalled();
  }));
});
