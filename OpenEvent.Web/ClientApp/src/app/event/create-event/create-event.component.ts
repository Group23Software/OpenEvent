import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {MatDialog} from "@angular/material/dialog";
import {CreateEventBody, EventDetailModel} from "../../_models/Event";
import {UserService} from "../../_Services/user.service";
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

  public isEditable: boolean = true;

  public ImagesAndSocialsForm = new FormGroup({
    socialLinks: new FormControl([]),
    thumbnail: new FormControl('', [Validators.required]),
    images: new FormControl([], [Validators.required]),
  });

  public markdown: string;

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
      this.eventPreview = {
        Address: this.addressForm.value,
        Categories: this.createEventForm.controls.Categories.value,
        Description: this.createEventForm.controls.Description.value,
        EndLocal: this.DateForm.controls.EndLocal.value,
        EndUTC: undefined,
        Id: "",
        Images: this.ImagesAndSocialsForm.controls.images.value,
        IsOnline: this.IsOnline.value == true,
        Name: this.createEventForm.controls.Name.value,
        Price: this.createEventForm.controls.Price.value,
        SocialLinks: this.ImagesAndSocialsForm.controls.socialLinks.value,
        StartLocal: this.DateForm.controls.StartLocal.value,
        StartUTC: undefined,
        Thumbnail: this.ImagesAndSocialsForm.controls.thumbnail.value,
        TicketsLeft: this.createEventForm.controls.NumberOfTickets.value,
        Promos: null,
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

    let createEventBody: CreateEventBody = {
      Address: this.addressForm.value != null ? this.addressForm.value: null,
      Categories: this.createEventForm.controls.Categories.value,
      Description: this.createEventForm.controls.Description.value,
      EndLocal: this.DateForm.controls.EndLocal.value,
      HostId: this.userService.User.Id,
      Images: this.ImagesAndSocialsForm.controls.images.value,
      IsOnline: this.IsOnline.value == true,
      Name: this.createEventForm.controls.Name.value,
      NumberOfTickets: this.createEventForm.controls.NumberOfTickets.value,
      Price: this.createEventForm.controls.Price.value * 100,
      SocialLinks: this.ImagesAndSocialsForm.controls.socialLinks.value,
      StartLocal: this.DateForm.controls.StartLocal.value,
      Thumbnail: this.ImagesAndSocialsForm.controls.thumbnail.value,
    }

    this.eventService.Create(createEventBody).subscribe(response =>
    {
      this.loading = false;

      // navigate to the event once created
      this.router.navigate(['/event', response.Id])
    }, (error: HttpErrorResponse) =>
    {
      console.error(error);
      this.CreateError = error.error.Message;
      this.loading = false;
    });
  }
}
