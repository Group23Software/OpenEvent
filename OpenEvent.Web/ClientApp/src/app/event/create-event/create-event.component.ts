import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ImageViewModel} from "../../_models/Image";
import {MatDialog} from "@angular/material/dialog";
import {ImageUploadComponent, uploadConfig} from "../../_extensions/image-upload/image-upload.component";
import {testImg} from "./TestImage";
import {CreateEventBody, EventDetailModel} from "../../_models/Event";
import {UserService} from "../../_Services/user.service";
import {SocialMedia} from "../../_models/SocialMedia";
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Category} from "../../_models/Category";
import {StepperSelectionEvent} from "@angular/cdk/stepper";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.component.html',
  styleUrls: ['./create-event.component.css']
})
export class CreateEventComponent implements OnInit
{

  public categories: Category[] = [];
  public thumbnail: ImageViewModel;
  public minDate: Date;
  public defaultTime: number[];
  public CreateError: string;
  public loading: boolean = false;
  public categoryStore: Category[] = [];
  public eventPreview: EventDetailModel = null;
  public gettingCategoriesError: string;

  public createEventForm = new FormGroup({
    Name: new FormControl('', [Validators.required]),
    Description: new FormControl('', [Validators.required]),
    Price: new FormControl(10, [Validators.required]),
    NumberOfTickets: new FormControl(10, [Validators.required]),
  });

  public DateForm = new FormGroup({
    StartLocal: new FormControl('', [Validators.required]),
    EndLocal: new FormControl('', [Validators.required]),
  })

  public IsOnline = new FormControl(false);

  public addressForm = new FormGroup({
    AddressLine1: new FormControl('', [Validators.required]),
    AddressLine2: new FormControl(''),
    City: new FormControl('', [Validators.required]),
    PostalCode: new FormControl('', [Validators.required, Validators.pattern('([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\\s?[0-9][A-Za-z]{2})')]),
    CountryCode: new FormControl('GB'),
    CountryName: new FormControl('United Kingdom'),
  })

  public SocialLinks = new FormGroup({
    Site: new FormControl(''),
    Instagram: new FormControl(''),
    Twitter: new FormControl(''),
    Facebook: new FormControl(''),
    Reddit: new FormControl('')
  })
  public isEditable: boolean = true;
  public eventImages: ImageViewModel[] = [
    {
      Label: "slippers",
      Source: testImg
    },
    {
      Label: "slippers",
      Source: testImg
    },
    {
      Label: "slippers",
      Source: testImg
    },
    {
      Label: "slippers",
      Source: testImg
    },
    {
      Label: "slippers",
      Source: testImg
    }
  ];


  constructor (private dialog: MatDialog, private userService: UserService, private eventService: EventService, private router: Router)
  {
    this.minDate = new Date();
    this.defaultTime = [new Date().getHours() + 1, 0, 0];
  }

  ngOnInit ()
  {
    this.eventService.GetAllCategories().subscribe(x => this.categoryStore = x, error =>
    {
      this.gettingCategoriesError = error.error.Message;
      console.error(error);
    });
  }

  loadEventDate (event: StepperSelectionEvent)
  {
    if (event.selectedIndex == 3)
    {
      console.log('updating preview data');
      this.eventPreview = {
        Address: {
          AddressLine1: this.addressForm.controls.AddressLine1.value,
          AddressLine2: this.addressForm.controls.AddressLine2.value,
          City: this.addressForm.controls.City.value,
          CountryCode: this.addressForm.controls.CountryCode.value,
          CountryName: this.addressForm.controls.CountryName.value,
          PostalCode: this.addressForm.controls.PostalCode.value
        },
        Categories: this.categories,
        Description: this.createEventForm.controls.Description.value,
        EndLocal: this.DateForm.controls.EndLocal.value,
        EndUTC: undefined,
        Id: "",
        Images: this.eventImages,
        IsOnline: this.IsOnline.value == true,
        Name: this.createEventForm.controls.Name.value,
        Price: this.createEventForm.controls.Price.value,
        SocialLinks: [
          {SocialMedia: SocialMedia.Site, Link: this.SocialLinks.controls.Site.value},
          {SocialMedia: SocialMedia.Instagram, Link: this.SocialLinks.controls.Instagram.value},
          {SocialMedia: SocialMedia.Twitter, Link: this.SocialLinks.controls.Twitter.value},
          {SocialMedia: SocialMedia.Facebook, Link: this.SocialLinks.controls.Facebook.value},
          {SocialMedia: SocialMedia.Reddit, Link: this.SocialLinks.controls.Reddit.value},
        ],
        StartLocal: this.DateForm.controls.StartLocal.value,
        StartUTC: undefined,
        Thumbnail: this.thumbnail,
        TicketsLeft: this.createEventForm.controls.NumberOfTickets.value
      }
    }
  }

  public clickedOnline ()
  {
    if (!this.IsOnline.value)
    {
      for (let control in this.addressForm.controls)
      {
        this.addressForm.controls[control].disable();
      }
    } else
    {
      for (let control in this.addressForm.controls)
      {
        this.addressForm.controls[control].enable();
      }
    }

  }

  create ()
  {
    this.loading = true;
    console.log(this);
    console.log(this.DateForm);

    let createEventBody: CreateEventBody = {
      Address: {
        AddressLine1: this.addressForm.controls.AddressLine1.value,
        AddressLine2: this.addressForm.controls.AddressLine2.value,
        City: this.addressForm.controls.City.value,
        CountryCode: this.addressForm.controls.CountryCode.value,
        CountryName: this.addressForm.controls.CountryName.value,
        PostalCode: this.addressForm.controls.PostalCode.value
      },
      Categories: this.categories,
      Description: this.createEventForm.controls.Description.value,
      EndLocal: this.DateForm.controls.EndLocal.value,
      HostId: this.userService.User.Id,
      Images: this.eventImages,
      IsOnline: this.IsOnline.value == true,
      Name: this.createEventForm.controls.Name.value,
      NumberOfTickets: this.createEventForm.controls.NumberOfTickets.value,
      Price: this.createEventForm.controls.Price.value,
      SocialLinks: [
        {SocialMedia: SocialMedia.Site, Link: this.SocialLinks.controls.Site.value},
        {SocialMedia: SocialMedia.Instagram, Link: this.SocialLinks.controls.Instagram.value},
        {SocialMedia: SocialMedia.Twitter, Link: this.SocialLinks.controls.Twitter.value},
        {SocialMedia: SocialMedia.Facebook, Link: this.SocialLinks.controls.Facebook.value},
        {SocialMedia: SocialMedia.Reddit, Link: this.SocialLinks.controls.Reddit.value},
      ],
      StartLocal: this.DateForm.controls.StartLocal.value,
      Thumbnail: this.thumbnail
    }
    console.log(createEventBody);

    this.eventService.Create(createEventBody).subscribe(response =>
    {
      console.log(response);
      this.loading = false;
      this.router.navigate(['/event',response.Id])
      //TODO: Redirect to event page
    }, (error: HttpErrorResponse) =>
    {
      console.error(error);
      this.CreateError = error.error.Message;
      this.loading = false;
    });
  }
}
