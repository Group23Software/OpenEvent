import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.component.html',
  styleUrls: ['./create-event.component.css']
})
export class CreateEventComponent implements OnInit
{

  public createEventForm = new FormGroup({
    Name: new FormControl(''),
    Description: new FormControl(''),
    Thumbnail: new FormControl(''),
    Date: new FormGroup({
      StartLocal: new FormControl(''),
      EndLocal: new FormControl(''),
    }),
    Price: new FormControl(''),
    IsOnline: new FormControl(''),
    NumberOfTickets: new FormControl(''),
    Categories: new FormControl('')
  });

  public addressForm = new FormGroup({
    AddressLine1: new FormControl(''),
    AddressLine2: new FormControl(''),
    PostalCode: new FormControl(''),
    CountryCode: new FormControl(''),
    CountryName: new FormControl(''),
  })

  public extraForm = new FormGroup({
    Images: new FormControl(''),
    SocialLinks: new FormControl(''),
  })
  public isEditable: boolean = true;

  constructor ()
  {
  }

  ngOnInit ()
  {
  }

}
