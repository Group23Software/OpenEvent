import {Component, ComponentRef, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {InOutAnimation} from "../animations";
import {animate, AnimationBuilder, AnimationPlayer, style} from "@angular/animations";

export enum IteratorStatus
{
  default = "black",
  bad = "red",
  good = "green"
}

@Component({
  selector: 'iterator',
  templateUrl: './iterator.component.html',
  styleUrls: ['./iterator.component.css'],
  animations: InOutAnimation
})
export class IteratorComponent implements OnInit, OnDestroy
{

  public ref: ComponentRef<IteratorComponent>;
  private player: AnimationPlayer;
  private timer;

  @ViewChild('hasAnimation') elementRef: ElementRef;

  public message: string = 'default message default message default message default message default message';
  public duration: number = 1000;
  public forever: boolean = false;
  public status: IteratorStatus = IteratorStatus.default;

  constructor (public animationBuilder: AnimationBuilder)
  {
  }

  ngOnInit (): void
  {
    if (!this.forever)
    {
      this.timer = setInterval(() =>
      {
        this.close();
      }, this.duration);
    }
  }

  public createAnimationPlayer ()
  {
    console.log('creating animation for', this.elementRef);
    if (this.player) this.player.destroy();

    let animationFactory = this.animationBuilder.build([animate(200, style({transform: 'translateX(100%)'}))]);

    this.player = animationFactory.create(this.elementRef.nativeElement);
  }

  public close ()
  {
    this.createAnimationPlayer();
    this.player.play();
    this.player.onDone(() => this.ngOnDestroy());
  }

  ngOnDestroy (): void
  {
    this.player = null;
    clearInterval(this.timer);
    this.ref.destroy();
  }

  // ngOnDestroy() {
  //   this.componentRef.destroy();
  // }
}
