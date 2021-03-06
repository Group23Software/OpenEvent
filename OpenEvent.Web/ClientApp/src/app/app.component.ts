import {
  AfterViewInit,
  Component,
  ComponentFactoryResolver,
  ElementRef,
  OnInit,
  ViewChild,
  ViewContainerRef
} from '@angular/core';
import {TriggerService} from "./_Services/trigger.service";
import {InOutAnimation} from "./_extensions/animations";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  animations: InOutAnimation
})
export class AppComponent implements OnInit, AfterViewInit
{

  @ViewChild('LupercalsCourt', { read: ViewContainerRef }) container: ViewContainerRef;

  public isDark: boolean;

  constructor (private trigger: TriggerService)
  {
    trigger.isDark.subscribe(is => this.isDark = is);
  }

  ngOnInit (): void
  {
  }

  ngAfterViewInit ()
  {
    console.log('container', this.container);
    this.trigger.Container = this.container;
  }
}
