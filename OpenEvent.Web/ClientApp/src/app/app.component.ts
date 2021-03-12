import {AfterViewInit, Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {TriggerService} from "./_Services/trigger.service";
import {InOutAnimation} from "./_extensions/animations";
import {UserService} from "./_Services/user.service";
import {map} from "rxjs/operators";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  animations: InOutAnimation
})
export class AppComponent implements OnInit, AfterViewInit
{

  @ViewChild('LupercalsCourt', { read: ViewContainerRef }) container: ViewContainerRef;

  public isDark: boolean;

  get IsLogged()
  {
    return this.userService.GetUserAsync().pipe(map(x => x != null));
  }

  constructor (private trigger: TriggerService, private userService: UserService)
  {
    trigger.isDark.subscribe(is => this.isDark = is);
    // authService.IsAuthenticated().subscribe(x => this.authed = x);
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
