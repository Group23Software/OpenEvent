import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute} from "@angular/router";
import {EventHostModel, UpdateEventBody} from "../../_models/Event";
import {EventService} from "../../_Services/event.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Category, CategoryViewModel} from "../../_models/Category";
import {SocialMedia} from "../../_models/SocialMedia";
import {ImageViewModel} from "../../_models/Image";
import {ImageUploadComponent, uploadConfig} from "../../_extensions/image-upload/image-upload.component";
import {MatDialog} from "@angular/material/dialog";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-event-config',
  templateUrl: './event-config.component.html',
  styleUrls: ['./event-config.component.css']
})
export class EventConfigComponent implements OnInit
{

  public event: EventHostModel;
  public categoryStore: Category[];
  public categories: Category[];

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
  public loading: boolean = false;
  public minDate: Date;

  constructor (private route: ActivatedRoute, private eventService: EventService,private dialog: MatDialog, private snackBar: MatSnackBar)
  {
    this.minDate = new Date();
  }

  ngOnInit (): void
  {
    this.eventService.GetAllCategories().subscribe(x =>
    {
      this.categoryStore = x;
      const id = this.route.snapshot.paramMap.get('id');
      if (this.eventService.HostsEvents)
      {
        this.event = this.eventService.HostsEvents.find(x => x.Id == id);
        this.loadFormData();
      }
      console.log(this.event);

      if (this.event == null)
      {
        this.eventService.GetForHost(id).subscribe(response =>
        {
          this.event = response;
          this.loadFormData();
        }, (e: HttpErrorResponse) =>
        {
          console.error(e);
        })
      }
    }, error => console.error(error));
  }

  private loadFormData ()
  {
    this.categories = this.categoryStore.filter(c => this.event.Categories.find(eC => eC.Name == c.Name));
    this.categoryStore = this.categoryStore.filter(c => !this.event.Categories.find(eC => eC.Name == c.Name))
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
      for (let control in this.addressForm.controls)
      {
        this.addressForm.controls[control].disable();
      }
    }
    console.log(this.event);
    this.Site.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Site).Link);
    this.Instagram.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Instagram).Link);
    this.Facebook.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Facebook).Link);
    this.Twitter.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Twitter).Link);
    this.Reddit.setValue(this.event.SocialLinks.find(x => x.SocialMedia == SocialMedia.Reddit).Link);
  }

  public addCategory (category: Category)
  {
    this.categories.push(category);
    this.categoryStore = this.categoryStore.filter(x => x.Id != category.Id);
  }

  public removeCategory (category: Category)
  {
    this.categoryStore.push(category);
    this.categories = this.categories.filter(x => x.Id != category.Id);
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
    this.loading = true;
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
      Thumbnail: this.event.Thumbnail
    }
    console.log(updateEvent);
    this.eventService.Update(updateEvent).subscribe(response =>
    {
      this.loading = false;
      this.snackBar.open('Updated event', 'close', {duration: 500});
    }, (e: HttpErrorResponse) => {
      this.loading = false;
    });
  }

  public imageUpload ()
  {
    let ref = this.dialog.open(ImageUploadComponent, {
      data: {
        height: 3,
        width: 4
      } as uploadConfig
    });

    ref.afterClosed().subscribe((image: ImageViewModel) =>
    {
      if (image)
      {
        this.event.Images.push(image);
      }
    });
  }

  public thumbnailUpload ()
  {
    let ref = this.dialog.open(ImageUploadComponent, {
      data: {
        height: 3,
        width: 4
      } as uploadConfig
    });

    ref.afterClosed().subscribe((image: ImageViewModel) =>
    {
      if (image)
      {
        this.event.Thumbnail = image;
      }
    });
  }

  public removeImage (image: ImageViewModel)
  {
    this.event.Images = this.event.Images.filter(x => x != image);
  }
}
