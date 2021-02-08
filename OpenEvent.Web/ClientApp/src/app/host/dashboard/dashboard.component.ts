import {AfterViewChecked, AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {EventDetailModel, EventHostModel, EventViewModel} from "../../_models/Event";
import {MatDrawerContainer} from "@angular/material/sidenav";
import {Router} from "@angular/router";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, AfterViewChecked
{
  public eventPreview: EventDetailModel;
  // private events: EventHostModel[];
  private updated: boolean = false;

  @ViewChild(MatDrawerContainer) sideNavContainer: MatDrawerContainer

  get Events ()
  {
    return this.eventService.HostsEvents;
  }

  constructor (private eventService: EventService, private router: Router)
  {
  }

  ngOnInit (): void
  {
    this.eventService.GetAllHosts().subscribe(response =>
    {
      if (this.Events)
      {
        this.eventPreview = this.Events[0];
      }
    }, (e: HttpErrorResponse) =>
    {
      console.error(e);
    });
  }

  public togglePreview (event: EventHostModel)
  {
    this.eventPreview = event;
  }

  ngAfterViewChecked (): void
  {
    if (!this.updated && this.eventPreview)
    {
      this.updated = true;
      console.log(this.sideNavContainer);
      this.sideNavContainer.updateContentMargins();
    }
  }

  public navigateToConfig (event: EventHostModel)
  {
    this.router.navigate(['/host/config/', event.Id], {state: event});
  }
}
