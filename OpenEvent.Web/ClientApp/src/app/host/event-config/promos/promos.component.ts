import {Component, Input, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {CreatePromoComponent} from "./create-promo/create-promo.component";
import {EventHostModel} from "../../../_models/Event";
import {MatSlideToggleChange} from "@angular/material/slide-toggle";
import {PromoService} from "../../../_Services/promo.service";
import {Promo} from "../../../_models/Promo";
import {HttpErrorResponse} from "@angular/common/http";
import {TriggerService} from "../../../_Services/trigger.service";
import {IteratorStatus} from "../../../_extensions/iterator/iterator.component";

@Component({
  selector: 'promos',
  templateUrl: './promos.component.html',
  styleUrls: ['./promos.component.css']
})
export class PromosComponent implements OnInit
{
  @Input() Event: EventHostModel;
  private error: string;

  constructor (private matDialog: MatDialog, private promoService: PromoService, private trigger: TriggerService)
  {
  }

  ngOnInit (): void
  {

  }

  public create ()
  {
    let ref = this.matDialog.open(CreatePromoComponent, {
      data: {
        event: this.Event
      }
    });

    ref.afterClosed().subscribe(r => this.Event.Promos.push(r));
  }

  public delete (promo: Promo)
  {
    this.promoService.Destroy(promo.Id).subscribe(r =>
    {
      this.Event.Promos = this.Event.Promos.filter(x => x.Id != promo.Id);
      this.trigger.Iterate("Removed promo", 2000, IteratorStatus.good);
    }, (e: HttpErrorResponse) => this.error = e.message);
  }

  public activeToggle (event: MatSlideToggleChange, promo: Promo)
  {
    this.promoService.Update({
      Active: event.checked,
      Discount: promo.Discount,
      End: promo.End,
      Start: promo.Start,
      Id: promo.Id
    }).subscribe(r =>
    {
      this.Event.Promos[this.Event.Promos.findIndex(x => x.Id == promo.Id)] = r;
      this.trigger.Iterate("Updated promo", 2000, IteratorStatus.good);
    }, (e: HttpErrorResponse) => this.error = e.message);
  }
}
