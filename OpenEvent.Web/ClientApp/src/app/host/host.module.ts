import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DashboardComponent} from "./dashboard/dashboard.component";
import {EventConfigComponent} from "./event-config/event-config.component";
import {VerifyDialog, VerifyTicketComponent} from "./verify-ticket/verify-ticket.component";
import {ImageUploadComponent} from "../_extensions/image-upload/image-upload.component";
import {CreateEventComponent} from "../event/create-event/create-event.component";
import {SharedModule} from "../shared.module";
import {MatSidenavModule} from "@angular/material/sidenav";
import {RouterModule} from "@angular/router";
import {MatDialogModule} from "@angular/material/dialog";
import {ZXingScannerModule} from "@zxing/ngx-scanner";



@NgModule({
  declarations: [
    CreateEventComponent,
    ImageUploadComponent,
    DashboardComponent,
    EventConfigComponent,
    VerifyTicketComponent,
    VerifyDialog
  ],
  imports: [
    CommonModule,
    SharedModule,
    MatSidenavModule,
    RouterModule.forChild([
      {path: 'create', component: CreateEventComponent},
      {path: 'dashboard', component: DashboardComponent},
      {path: 'config/:id', component: EventConfigComponent},
      {path: 'verify/:id', component: VerifyTicketComponent}
    ]),
    MatDialogModule,
    ZXingScannerModule
  ]
})
export class HostModule
{
}
