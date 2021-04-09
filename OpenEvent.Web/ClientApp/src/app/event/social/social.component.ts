import {Component, Input, OnInit} from '@angular/core';
import {SocialLinkViewModel} from "../../_models/SocialLink";
import {SocialMedia} from "../../_models/SocialMedia";

@Component({
  selector: 'social',
  templateUrl: './social.component.html',
  styleUrls: ['./social.component.css']
})
export class SocialComponent implements OnInit
{
  @Input() socialString: string;
  @Input() socialLink: SocialLinkViewModel;
  public image: string;

  constructor ()
  {
  }

  ngOnInit (): void
  {
    // console.log(this.socialLink,this.socialString);

    if (this.socialString != null)
    {
      this.socialLink = {SocialMedia: SocialMedia[this.socialString], Link: ''}
    };

    if (this.socialLink)
    {
      switch (this.socialLink.SocialMedia)
      {
        case (SocialMedia.Site):
          this.image = 'assets/site.svg';
          break;
        case (SocialMedia.Instagram):
          this.image = 'assets/instagram.svg';
          break;
        case (SocialMedia.Twitter):
          this.image = 'assets/twitter.svg';
          break;
        case (SocialMedia.Facebook):
          this.image = 'assets/facebook.svg';
          break;
        case (SocialMedia.Reddit):
          this.image = 'assets/reddit.svg';
          break;
      }
    }
  }
}
