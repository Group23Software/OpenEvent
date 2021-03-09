import {Component, Input, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {CreatePromoComponent} from "./create-promo/create-promo.component";
import {EventHostModel} from "../../../_models/Event";

@Component({
  selector: 'promos',
  templateUrl: './promos.component.html',
  styleUrls: ['./promos.component.css']
})
export class PromosComponent implements OnInit
{
  @Input() Event: EventHostModel;

  constructor (private matDialog: MatDialog)
  {
  }

  ngOnInit (): void
  {

  }

  public create ()
  {
    this.matDialog.open(CreatePromoComponent, {
      data: {
        event: this.Event
      }
    });
  }
}
