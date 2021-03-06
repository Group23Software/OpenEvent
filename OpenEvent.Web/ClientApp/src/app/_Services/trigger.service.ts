import {
  ComponentFactoryResolver,
  EventEmitter,
  Injectable,
  ViewContainerRef
} from '@angular/core';
import {IteratorComponent, IteratorStatus} from "../_extensions/iterator/iterator.component";

@Injectable({
  providedIn: 'root'
})
export class TriggerService
{
  private container: ViewContainerRef;

  set Container (container)
  {
    this.container = container
  }

  public isDark: EventEmitter<boolean> = new EventEmitter<boolean>();
  public loading: EventEmitter<boolean> = new EventEmitter<boolean>();


  constructor (private componentFactoryResolver: ComponentFactoryResolver)
  {
  }

  public IterateForever (message: string, status?: IteratorStatus)
  {
    this.Iterate(message, null, status, true)
  }

  public Iterate (message?: string, duration?: number, status?: IteratorStatus, forever?: boolean)
  {
    const factory = this.componentFactoryResolver.resolveComponentFactory(IteratorComponent);
    const component = this.container.createComponent(factory);
    component.instance.ref = component;

    if (message) component.instance.message = message;
    if (duration) component.instance.duration = duration;
    if (forever) component.instance.forever = forever;
    if (status) component.instance.status = status;

    this.container.insert(component.hostView);
    component.onDestroy(() => console.log('the iterator has been destroyed'));
  }

}
