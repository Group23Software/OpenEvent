import {AfterViewChecked, AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {EventDetailModel, EventHostModel, EventViewModel} from "../../_models/Event";
import {MatDrawerContainer} from "@angular/material/sidenav";
import {Router} from "@angular/router";
import {ConfirmDialogComponent} from "../../_extensions/confirm-dialog/confirm-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {InOutAnimation} from "../../_extensions/animations";
import {MappedEventAnalytics} from "../../_models/Analytic";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  animations: InOutAnimation
})
export class DashboardComponent implements OnInit, AfterViewChecked
{
  public eventPreview: EventHostModel;
  public finishedEventAnalytics: MappedEventAnalytics;

  private updated: boolean = false;
  public cancelingEventError: string;
  public gettingEventsError: string;

  @ViewChild(MatDrawerContainer) sideNavContainer: MatDrawerContainer
  public loading: boolean = true;
  today: Date;
  private error: string;

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
        if (this.eventPreview.Finished) {
          this.loading = true;
          this.eventService.GetAnalytics(this.eventPreview.Id).subscribe(analytics => this.finishedEventAnalytics = analytics,(e: HttpErrorResponse) => this.gettingEventsError = e.error.Message,() => this.loading = false);
        }
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
    if (this.eventPreview.Finished) {
      this.loading = true;
      this.eventService.GetAnalytics(this.eventPreview.Id).subscribe(analytics => this.finishedEventAnalytics = analytics,(e: HttpErrorResponse) => this.gettingEventsError = e.error.Message,() => this.loading = false);
    }
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

  public cancelEvent (id: string)
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

  public navigateToVerify ()
  {
    this.router.navigate(['/host/verify',this.eventPreview.Id]);
  }

  public hasEnded (event: EventDetailModel) : boolean
  {
    return new Date(event.EndLocal) < new Date();
  }

  public finishEvent () : void
  {
    this.loading = true;
    this.eventService.Update({
      ...this.eventPreview,
      Finished: true,
    }).subscribe(() => {
      this.eventService.HostsEvents.find(x => x.Id == this.eventPreview.Id).Finished = true;
      this.togglePreview(this.eventPreview);
    },(e: HttpErrorResponse) => this.error = e.error.Message,() => this.loading = false);
  }
}
