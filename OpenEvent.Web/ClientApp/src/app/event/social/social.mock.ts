import {Component, Input, OnInit} from "@angular/core";
import {SocialLinkViewModel} from "../../_models/SocialLink";

@Component({
  selector: 'social',
  template: ''
})
export class MockSocialComponent implements OnInit
{
  @Input() socialString: string;
  @Input() socialLink: SocialLinkViewModel;
  ngOnInit (): void
  {
  }
}
