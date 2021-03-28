import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SocialLinksFormComponent } from './social-links-form.component';
import {MatSelectModule} from "@angular/material/select";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatIconModule} from "@angular/material/icon";
import {MockSocialComponent} from "../../social/social.mock";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";

describe('SocialLinksFormComponent', () => {
  let component: SocialLinksFormComponent;
  let fixture: ComponentFixture<SocialLinksFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        MatSelectModule,
        MatFormFieldModule,
        MatInputModule,
        FormsModule,
        ReactiveFormsModule,
        MatIconModule,
        BrowserAnimationsModule
      ],
      declarations: [
        SocialLinksFormComponent,
        MockSocialComponent
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SocialLinksFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
