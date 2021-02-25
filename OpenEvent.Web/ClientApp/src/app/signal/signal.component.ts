import {Component, Input, OnInit} from '@angular/core';
import {Signal} from "./Signal";

@Component({
  selector: 'signal',
  templateUrl: './signal.component.html',
  styleUrls: ['./signal.component.css']
})
export class SignalComponent implements OnInit
{

  @Input() signal: Signal;

  constructor ()
  {
  }

  ngOnInit (): void
  {
  }

}
