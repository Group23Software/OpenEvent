import {Component, Input, OnInit} from '@angular/core';
import {PopularEventViewModel} from "../../_models/Event";

@Component({
  selector: 'popular-events',
  templateUrl: './popular-events.component.html',
  styleUrls: ['./popular-events.component.css']
})
export class PopularEventsComponent implements OnInit
{

  @Input() PopularEvents: PopularEventViewModel[];

  constructor ()
  {
  }

  ngOnInit (): void
  {
  }

}
