import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute} from "@angular/router";
import {EventHostModel, UpdateEventBody} from "../../_models/Event";
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Category} from "../../_models/Category";
import {SocialMedia} from "../../_models/SocialMedia";
import {MappedEventAnalytics} from "../../_models/Analytic";
import {TriggerService} from "../../_Services/trigger.service";
import {IteratorStatus} from "../../_extensions/iterator/iterator.component";

@Component({
  selector: 'app-event-config',
  templateUrl: './event-config.component.html',
  styleUrls: ['./event-config.component.css']
})
export class EventConfigComponent implements OnInit
{

  public event: EventHostModel = null;
  public analytics: MappedEventAnalytics;
  public categoryStore: Category[] = [];
  public categories: Category[] = [];

  public Name = new FormControl('', [Validators.required]);
  public Description = new FormControl('', [Validators.required]);
  public Price = new FormControl(10, [Validators.required]);
  public NumberOfTickets = new FormControl(10, [Validators.required]);
  public StartLocal = new FormControl('', [Validators.required]);
  public EndLocal = new FormControl('', [Validators.required]);

  public Site = new FormControl('');
  public Instagram = new FormControl('');
  public Twitter = new FormControl('');
  public Facebook = new FormControl('');
  public Reddit = new FormControl('');

  public IsOnline = new FormControl();
  public addressForm = new FormGroup({
    AddressLine1: new FormControl('', [Validators.required]),
    AddressLine2: new FormControl(''),
    City: new FormControl('', [Validators.required]),
    PostalCode: new FormControl('', [Validators.required, Validators.pattern('([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\\s?[0-9][A-Za-z]{2})')]),
    CountryCode: new FormControl('GB'),
    CountryName: new FormControl('United Kingdom'),
  })
  public updatingEvent: boolean = false;
  public gettingEventError: string;
  public updateEventError: string;
  public gettingCategoriesError: string;
  public minDate: Date;
  public loading: boolean = true;
  markdown: string;
  transactionColumns = ['stripeId', 'status', 'start', 'end', 'amount', 'paid'];
  defaultDate = "0001-01-01T00:00:00";

  get CompletedTransactionWorth () {
    return this.event.Transactions.filter(x => x.Paid).reduce((total,x) => total + x.Amount,0);
  }

  get PendingTransactionWorth ()
  {
   return this.event.Transactions.filter(x => !x.Paid).reduce((total,x) => total + x.Amount,0);
  }


  constructor (private route: ActivatedRoute, private eventService: EventService, private trigger: TriggerService)
  {
    this.minDate = new Date();
  }

  ngOnInit (): void
  {
    this.eventService.GetAllCategories().subscribe(x =>
    {
      this.categoryStore = x;
      const id = this.route.snapshot.paramMap.get('id');
      this.eventService.GetAnalytics(id).subscribe(analytics => this.analytics = analytics)
      // if (this.eventService.HostsEvents)
      // {
      //   this.event = this.eventService.HostsEvents.find(x => x.Id == id);
      //   this.loadFormData();
      // }

      if (this.event == null)
      {
        this.eventService.GetForHost(id).subscribe(response =>
        {
          this.event = response;
          this.loadFormData();
        }, (e: HttpErrorResponse) =>
        {
          this.gettingEventError = e.error.Message;
        })
      }
    }, (error: HttpErrorResponse) =>
    {
      this.gettingCategoriesError = error.error.Message;
      console.error(error);
    });
  }

  private loadFormData ()
  {
    this.loading = false;
    this.categories = this.categoryStore != null && this.categoryStore.length > 0 ? this.categoryStore.filter(c => this.event.Categories.find(eC => eC.Name == c.Name)) : [];
    this.categoryStore = this.categoryStore != null && this.categoryStore.length > 0 ? this.categoryStore.filter(c => !this.event.Categories.find(eC => eC.Name == c.Name)) : [];
    this.Name.setValue(this.event.Name);
    this.Description.setValue(this.event.Description);
    this.Price.setValue(this.event.Price);
    this.NumberOfTickets.setValue(this.event.Tickets.length);
    this.StartLocal.setValue(this.event.StartLocal);
    this.EndLocal.setValue(this.event.EndLocal);
    this.IsOnline.setValue(this.event.IsOnline);
    this.addressForm.setValue(this.event.Address);
    if (!this.IsOnline)
    {
      this.addressForm.disable();
      // for (let control in this.addressForm.controls)
      // {
      //   this.addressForm.controls[control].disable();
      // }
    }
    // TODO: Fix social media links, this is nasty.
    // if (this.event.SocialLinks != null && this.event.SocialLinks.length > 0)
    // {
    //   this.Site.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Site).Link);
    //   this.Instagram.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Instagram).Link);
    //   this.Facebook.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Facebook).Link);
    //   this.Twitter.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Twitter).Link);
    //   this.Reddit.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Reddit).Link);
    // }
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

  public updateEvent ()
  {
    this.updatingEvent = true;
    let updateEvent: UpdateEventBody = {
      Address: {
        AddressLine1: this.addressForm.controls.AddressLine1.value,
        AddressLine2: this.addressForm.controls.AddressLine2.value,
        City: this.addressForm.controls.City.value,
        CountryCode: this.addressForm.controls.CountryCode.value,
        CountryName: this.addressForm.controls.CountryName.value,
        PostalCode: this.addressForm.controls.PostalCode.value
      },
      Categories: this.categories,
      Description: this.Description.value,
      EndLocal: this.EndLocal.value,
      Id: this.event.Id,
      Images: this.event.Images,
      IsOnline: this.IsOnline.value == true,
      Name: this.Name.value,
      Price: this.Price.value,
      SocialLinks: [
        {SocialMedia: SocialMedia.Site, Link: this.Site.value},
        {SocialMedia: SocialMedia.Instagram, Link: this.Instagram.value},
        {SocialMedia: SocialMedia.Twitter, Link: this.Twitter.value},
        {SocialMedia: SocialMedia.Facebook, Link: this.Facebook.value},
        {SocialMedia: SocialMedia.Reddit, Link: this.Reddit.value},
      ],
      StartLocal: this.StartLocal.value,
      Thumbnail: this.event.Thumbnail,
      Finished: this.event.Finished
    }
    console.log(updateEvent);
    this.eventService.Update(updateEvent).subscribe(response =>
    {
      this.updatingEvent = false;
      this.trigger.Iterate('Updated event', 1000, IteratorStatus.good);
    }, (e: HttpErrorResponse) =>
    {
      this.updatingEvent = false;
      this.updateEventError = e.error.Message;
    });
  }


}
