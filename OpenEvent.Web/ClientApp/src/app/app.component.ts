import { Component } from '@angular/core';
import {TriggerService} from "./_Services/trigger.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  public isDark: boolean;

  constructor (private trigger: TriggerService)
  {
    trigger.isDark.subscribe(is => this.isDark = is);
  }

}
