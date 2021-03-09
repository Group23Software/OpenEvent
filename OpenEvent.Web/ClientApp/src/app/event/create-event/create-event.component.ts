import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ImageViewModel} from "../../_models/Image";
import {MatDialog} from "@angular/material/dialog";
import {testImg} from "./TestImage";
import {CreateEventBody, EventDetailModel} from "../../_models/Event";
import {UserService} from "../../_Services/user.service";
import {SocialMedia} from "../../_models/SocialMedia";
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Category} from "../../_models/Category";
import {StepperSelectionEvent} from "@angular/cdk/stepper";
import {Router} from "@angular/router";
import {InOutAnimation} from "../../_extensions/animations";
import {map} from "rxjs/operators";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.component.html',
  styleUrls: ['./create-event.component.css'],
  animations: InOutAnimation
})
export class CreateEventComponent implements OnInit
{

  get UserHasBank ()
  {
    return this.userService.User?.BankAccounts?.length > 0
  }

  public thumbnail: ImageViewModel;
  public minDate: Date;
  public defaultTime: number[];
  public CreateError: string;
  public loading: boolean = false;
  public categoryStore: Category[] = [];
  public eventPreview: EventDetailModel = null;
  public getError: string;

  public createEventForm = new FormGroup({
    Name: new FormControl('', [Validators.required]),
    Description: new FormControl('', [Validators.required]),
    Price: new FormControl(10, [Validators.required]),
    NumberOfTickets: new FormControl(10, [Validators.required]),
    Categories: new FormControl('', [Validators.required])
  });

  public DateForm = new FormGroup({
    StartLocal: new FormControl('', [Validators.required]),
    EndLocal: new FormControl('', [Validators.required]),
  });

  public IsOnline = new FormControl(false);

  public addressForm = new FormControl();

  public SocialLinks = new FormGroup({
    Site: new FormControl(''),
    Instagram: new FormControl(''),
    Twitter: new FormControl(''),
    Facebook: new FormControl(''),
    Reddit: new FormControl('')
  });
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
  markdown: string;


  constructor (private dialog: MatDialog, private userService: UserService, private eventService: EventService, private router: Router)
  {
    this.minDate = new Date();
    this.defaultTime = [new Date().getHours() + 1, 0, 0];
  }

  ngOnInit ()
  {
    this.loading = true;
    let subs = [this.userService.NeedAccountUser(), this.eventService.GetAllCategories().pipe(map(x => this.categoryStore = x))];
    forkJoin(subs).subscribe(value =>
    {
    }, (e: HttpErrorResponse) => this.getError = e.error.Message, () => this.loading = false);
  }

  loadEventData (event: StepperSelectionEvent)
  {
    if (event.selectedIndex == 3)
    {
      console.log('updating preview data');
      this.eventPreview = {
        Address: this.addressForm.value,
        Categories: this.createEventForm.controls.Categories.value,
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
        TicketsLeft: this.createEventForm.controls.NumberOfTickets.value,
        Promos: null
      }
    }
  }

  public clickedOnline ()
  {
    this.IsOnline.value ? this.addressForm.enable() : this.addressForm.disable();
  }

  create ()
  {
    this.loading = true;
    console.log(this);
    console.log(this.DateForm);

    let createEventBody: CreateEventBody = {
      Address: this.addressForm.value != null ? this.addressForm.value: null,
      Categories: this.createEventForm.controls.Categories.value,
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
      this.router.navigate(['/event', response.Id])
    }, (error: HttpErrorResponse) =>
    {
      console.error(error);
      this.CreateError = error.error.Message;
      this.loading = false;
    });
  }
}
