import {Component, Input, OnInit} from '@angular/core';
import {EventViewModel, PopularEventViewModel} from "../../_models/Event";
import {Router} from "@angular/router";
import {EventService} from "../../_Services/event.service";

@Component({
  selector: 'event-preview',
  templateUrl: './event-preview.component.html',
  styleUrls: ['./event-preview.component.css']
})
export class EventPreviewComponent implements OnInit
{
  @Input() event: EventViewModel | PopularEventViewModel;
  @Input() showDownVote: boolean = true;

  constructor (private router: Router, private eventService: EventService)
  {
  }

  ngOnInit (): void
  {
  }

  public navigateToEvent ()
  {
    this.router.navigate(['/event',this.event.Id]);
  }

  downVote ()
  {
    console.log("down voting", this.event);
    this.eventService.DownVote(this.event.Id).subscribe();
  }
}
