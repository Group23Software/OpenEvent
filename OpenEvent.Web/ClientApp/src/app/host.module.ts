import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateEventComponent} from './event/create-event/create-event.component';
import {RouterModule} from "@angular/router";
import {SharedModule} from "./shared.module";
import {ImageUploadComponent} from "./_extensions/image-upload/image-upload.component";
import {DashboardComponent} from './host/dashboard/dashboard.component';
import {MatSidenavModule} from "@angular/material/sidenav";
import {EventConfigComponent} from "./host/event-config/event-config.component";
import {AuthGuard} from "./_guards/auth.guard";


@NgModule({
  declarations: [
    CreateEventComponent,
    ImageUploadComponent,
    DashboardComponent,
    EventConfigComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MatSidenavModule,
    RouterModule.forChild([
      {path: 'create', component: CreateEventComponent},
      {path: 'dashboard', component: DashboardComponent},
      {path: 'config/:id', component: EventConfigComponent}
    ]),
  ]
})
export class HostModule
{
}
