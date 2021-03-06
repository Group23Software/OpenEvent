import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MatInputModule} from "@angular/material/input";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatIconModule, MatIconRegistry} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatDialogModule} from "@angular/material/dialog";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatNativeDateModule} from "@angular/material/core";
import {ImageCropperModule} from "ngx-image-cropper";
import {MatDividerModule} from "@angular/material/divider";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatMenuModule} from "@angular/material/menu";
import {MatCardModule} from "@angular/material/card";
import {MatTabsModule} from "@angular/material/tabs";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatSlideToggleModule} from "@angular/material/slide-toggle";
import {MatStepperModule} from "@angular/material/stepper";
import {MatSliderModule} from "@angular/material/slider";
import {NgxMatDatetimePickerModule, NgxMatNativeDateModule} from "@angular-material-components/datetime-picker";
import {LoadingComponent} from "./loading/loading.component";
import {MatChipsModule} from "@angular/material/chips";
import {UserNavComponent} from "./navs/user-nav/user-nav.component";
import {IvyCarouselModule} from "angular-responsive-carousel";
import {SocialComponent} from './event/social/social.component';
import {SafePipeModule} from "safe-pipe";
import {RouterModule} from "@angular/router";
import {EventComponent} from "./event/event/event.component";
import {MatListModule} from "@angular/material/list";
import {MatExpansionModule} from "@angular/material/expansion";
import {DomSanitizer} from "@angular/platform-browser";
import {MatSelectModule} from "@angular/material/select";
import {CategoryListComponent} from './category-list/category-list.component';
import {ImageListComponent} from './image-list/image-list.component';
import {ThumbnailEditComponent} from "./thumbnail-edit/thumbnail-edit.component";
import {AddressFormComponent} from './address-form/address-form.component';
import {FlexLayoutModule} from "@angular/flex-layout";
import { SignalComponent } from './signal/signal.component';
import {MatCarouselModule} from "@ngbmodule/material-carousel";
import {GalleryModule} from "ng-gallery";
import {AngularFittextModule} from "angular-fittext";
import { TicketPurchaseDialogComponent } from './ticket/ticket-purchase-dialog/ticket-purchase-dialog.component';
import {VirtualCardComponent} from "./account/payment-methods/virtual-card/virtual-card.component";
import {MatTableModule} from "@angular/material/table";
import {MarkdownModule} from "ngx-markdown";
import { IteratorComponent } from './_extensions/iterator/iterator.component';


@NgModule({
  declarations: [
    LoadingComponent,
    SocialComponent,
    UserNavComponent,
    EventComponent,
    CategoryListComponent,
    ImageListComponent,
    ThumbnailEditComponent,
    AddressFormComponent,
    SignalComponent,
    TicketPurchaseDialogComponent,
    VirtualCardComponent,
    IteratorComponent
  ],
  imports: [
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    // BrowserAnimationsModule,

    CommonModule,
    RouterModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ImageCropperModule,
    MatDividerModule,
    MatSnackBarModule,
    MatToolbarModule,
    MatIconModule,
    MatMenuModule,
    MatCardModule,
    MatTabsModule,
    MatProgressBarModule,
    MatSlideToggleModule,
    MatStepperModule,
    MatSliderModule,
    NgxMatNativeDateModule,
    NgxMatDatetimePickerModule,
    MatChipsModule,
    IvyCarouselModule,
    SafePipeModule,
    MatListModule,
    MatExpansionModule,
    MatSelectModule,
    FlexLayoutModule,
    MatCarouselModule,
    GalleryModule,
    AngularFittextModule,
    // MatTableModule
    MatSelectModule,
    MarkdownModule.forRoot(),
  ],
  exports: [

    CommonModule,
    RouterModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    // BrowserAnimationsModule,

    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ImageCropperModule,
    MatDividerModule,
    MatSnackBarModule,
    MatToolbarModule,
    MatIconModule,
    MatMenuModule,
    MatCardModule,
    MatTabsModule,
    MatProgressBarModule,
    MatSlideToggleModule,
    MatStepperModule,
    MatSliderModule,
    NgxMatNativeDateModule,
    NgxMatDatetimePickerModule,
    MatChipsModule,
    IvyCarouselModule,
    SafePipeModule,
    MatListModule,
    MatExpansionModule,
    MatSelectModule,
    FlexLayoutModule,
    MatSelectModule,

    LoadingComponent,
    SocialComponent,
    UserNavComponent,
    EventComponent,
    CategoryListComponent,
    ImageListComponent,
    ThumbnailEditComponent,
    AddressFormComponent,
    SignalComponent,
    GalleryModule,
    AngularFittextModule,
    VirtualCardComponent,
    // MatTableModule
    MarkdownModule,
  ]
})
export class SharedModule
{
  constructor (iconRegistry: MatIconRegistry, sanitizer: DomSanitizer)
  {
    iconRegistry.addSvgIcon('instagram', sanitizer.bypassSecurityTrustResourceUrl('../assets/instagram.svg'));
    iconRegistry.addSvgIcon('twitter', sanitizer.bypassSecurityTrustResourceUrl('../assets/twitter.svg'));
    iconRegistry.addSvgIcon('facebook', sanitizer.bypassSecurityTrustResourceUrl('../assets/facebook.svg'));
    iconRegistry.addSvgIcon('reddit', sanitizer.bypassSecurityTrustResourceUrl('../assets/reddit.svg'));
    iconRegistry.addSvgIcon('site', sanitizer.bypassSecurityTrustResourceUrl('../assets/site.svg'));

    iconRegistry.addSvgIcon('Visa', sanitizer.bypassSecurityTrustResourceUrl('../assets/visa.svg'));
    iconRegistry.addSvgIcon('MasterCard', sanitizer.bypassSecurityTrustResourceUrl('../assets/mastercard.svg'));
    iconRegistry.addSvgIcon('American Express', sanitizer.bypassSecurityTrustResourceUrl('../assets/americanexpress.svg'));
  }
}
