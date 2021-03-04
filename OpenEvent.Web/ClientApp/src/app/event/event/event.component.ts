import {Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {EventDetailModel} from "../../_models/Event";
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Location} from '@angular/common';
import {GalleryItem, ImageItem} from "ng-gallery";
import {CreateAccountComponent} from "../../login/create-account/create-account.component";
import {MatDialog} from "@angular/material/dialog";
import {
  TicketPurchaseDialogComponent,
  TicketPurchaseDialogData
} from "../../ticket/ticket-purchase-dialog/ticket-purchase-dialog.component";
import {TransactionService} from "../../_Services/transaction.service";

@Component({
  selector: 'event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit, OnChanges
{
  @Input() EventPreview: EventDetailModel;
  private event: EventDetailModel;
  public mapLink: string;
  public eventImages: GalleryItem[];

  // get eventImages () : GalleryItem[]
  // {
  //   let images: ImageItem[] = this.Event.Images.map(x => new ImageItem({src: x.Source}));
  //   console.log(images);
  //   return images;
  // }

  get Event ()
  {
    return this.event;
  }

  constructor (private route: ActivatedRoute, private eventService: EventService, private location: Location, private dialog: MatDialog, private transactionService: TransactionService)
  {
  }

  ngOnInit (): void
  {
    if (!this.EventPreview)
    {
      const id = this.route.snapshot.paramMap.get('id');
      this.eventService.GetForPublic(id).subscribe(response =>
      {
        if (response)
        {
          this.event = response;
          if (this.event.Address && !this.event.IsOnline)
          {
            this.mapLink = this.createMapLink();
          }
          this.eventImages = this.Event.Images.map(x => new ImageItem({src: x.Source}));
        }
      }, (error: HttpErrorResponse) =>
      {
        this.back();
        console.error(error);
      });
    } else
    {
      this.event = this.EventPreview;
      if (this.event.Address && !this.event.IsOnline)
      {
        this.mapLink = this.createMapLink();
      }
      this.eventImages = this.Event.Images.map(x => new ImageItem({src: x.Source}));
    }
  }

  private createMapLink (): string
  {
    let query = encodeURI(`${this.event.Address.AddressLine1},${this.event.Address.City},${this.event.Address.CountryName},${this.event.Address.PostalCode}`);
    return `https://maps.google.com/maps?q=${query}&t=&z=13&ie=UTF8&iwloc=&output=embed`;
  }

  public back ()
  {
    this.location.back();
  }

  ngOnChanges (changes: SimpleChanges): void
  {
    this.ngOnInit();
  }

  public purchase ()
  {
    let dialog = this.dialog.open(TicketPurchaseDialogComponent, {
      maxWidth: "80vw",
      data: {
        Event: this.event
      } as TicketPurchaseDialogData
    });

    // TODO: Go to ticket
    dialog.afterClosed().subscribe(result => {
      if (result == undefined) this.transactionService.CancelIntent(this.Event.Id).subscribe();
      dialog = null;
    });
  }
}


