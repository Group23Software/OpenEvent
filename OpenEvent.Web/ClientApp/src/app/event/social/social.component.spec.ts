import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';

import {SocialComponent} from './social.component';
import {SocialMedia} from "../../_models/SocialMedia";

describe('SocialComponent', () =>
{
  let component: SocialComponent;
  let fixture: ComponentFixture<SocialComponent>;

  let filenames: { [key: string]: string } = {};
  filenames["Site"] = "assets/site.svg";
  filenames["Instagram"] = "assets/instagram.svg";
  filenames["Twitter"] = "assets/twitter.svg";
  filenames["Facebook"] = "assets/facebook.svg";
  filenames["Reddit"] = "assets/reddit.svg";


  beforeEach(async () =>
  {
    await TestBed.configureTestingModule({
      declarations: [SocialComponent]
    })
                 .compileComponents();
  });

  beforeEach(() =>
  {
    fixture = TestBed.createComponent(SocialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>
  {
    expect(component).toBeTruthy();
  });

  it('should get image', () =>
  {
    Object.keys(SocialMedia).filter(key => !isNaN(Number(SocialMedia[key]))).forEach(socialMedia =>
    {
      component.socialLink = {Link: "", SocialMedia: SocialMedia[socialMedia]}
      component.ngOnInit();
      fixture.detectChanges();
      expect(component.image).not.toBeNull();
      expect(component.image).toEqual(filenames[socialMedia]);
    });
  });
});
