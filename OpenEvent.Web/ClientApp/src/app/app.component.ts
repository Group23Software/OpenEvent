import { Component } from '@angular/core';
import {TriggerService} from "./_Services/trigger.service";
import {InOutAnimation} from "./_extensions/animations";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  animations: InOutAnimation
})
export class AppComponent {

  public isDark: boolean;

  constructor (private trigger: TriggerService)
  {
    trigger.isDark.subscribe(is => this.isDark = is);
  }
}
