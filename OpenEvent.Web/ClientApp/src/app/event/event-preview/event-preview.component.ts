import {Component, Input, OnInit} from '@angular/core';
import {EventViewModel} from "../../_models/Event";
import {Router} from "@angular/router";

@Component({
  selector: 'event-preview',
  templateUrl: './event-preview.component.html',
  styleUrls: ['./event-preview.component.css']
})
export class EventPreviewComponent implements OnInit
{
  @Input() event: EventViewModel;

  constructor (private router: Router)
  {
  }

  ngOnInit (): void
  {
  }

  public navigateToEvent ()
  {
    this.router.navigate(['/event',this.event.Id]);
  }
}
