import {AfterViewChecked, AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {EventDetailModel, EventHostModel, EventViewModel} from "../../_models/Event";
import {MatDrawerContainer} from "@angular/material/sidenav";
import {Router} from "@angular/router";
import {ConfirmDialogComponent} from "../../_extensions/confirm-dialog/confirm-dialog.component";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, AfterViewChecked
{
  public eventPreview: EventDetailModel;
  private updated: boolean = false;
  public cancelingEventError: string;
  public gettingEventsError: string;

  @ViewChild(MatDrawerContainer) sideNavContainer: MatDrawerContainer
  public loading: boolean = true;

  get Events ()
  {
    return this.eventService.HostsEvents;
  }

  constructor (private eventService: EventService, private router: Router, private dialog: MatDialog)
  {
  }

  ngOnInit (): void
  {
    this.eventService.GetAllHosts().subscribe(response =>
    {
      this.loading = false;
      if (this.Events)
      {
        this.eventPreview = this.Events[0];
      }
    }, (e: HttpErrorResponse) =>
    {
      this.gettingEventsError = e.error.Message;
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

  public navigateToConfig ()
  {
    let event = this.Events.find(x => x.Id == this.eventPreview.Id);
    this.router.navigate(['/host/config/', event.Id], {state: event});
  }

  cancelEvent (id: string)
  {
    let cancelEvent = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: `Are you sure you want to cancel "${this.eventPreview.Name}", this cannot be undone`,
        color: 'warn'
      }
    });

    cancelEvent.afterClosed().subscribe(result =>
    {
      if (result)
      {
        this.eventService.Cancel(id).subscribe(() =>
        {
          this.router.navigate(['/host/events']);
        }, (error: HttpErrorResponse) =>
        {
          this.cancelingEventError = error.error.Message;
          console.error(error);
        });
      }
    });
  }
}
