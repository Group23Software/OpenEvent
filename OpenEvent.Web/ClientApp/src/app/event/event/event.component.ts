import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {EventDetailModel} from "../../_models/Event";
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Location} from '@angular/common';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit
{

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
}


