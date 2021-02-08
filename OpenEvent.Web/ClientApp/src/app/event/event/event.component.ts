import {Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {EventDetailModel} from "../../_models/Event";
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Location} from '@angular/common';

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

  get Event ()
  {
    return this.event;
  }

  constructor (private route: ActivatedRoute, private eventService: EventService, private location: Location)
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
        }
      }, (error: HttpErrorResponse) =>
      {
        this.back();
        console.error(error);
      });
    } else {
      this.event = this.EventPreview;
      if (this.event.Address && !this.event.IsOnline)
      {
        this.mapLink = this.createMapLink();
      }
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
}


