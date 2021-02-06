import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateEventComponent} from './event/create-event/create-event.component';
import {RouterModule} from "@angular/router";
import {SharedModule} from "./shared.module";
import {UserNavComponent} from "./navs/user-nav/user-nav.component";
import {UserModule} from "./user.module";
import {ImageUploadComponent} from "./_extensions/image-upload/image-upload.component";
import {AppModule} from "./app.module";


@NgModule({
  declarations: [
    CreateEventComponent,
    ImageUploadComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    UserModule,
    RouterModule.forChild([
      {path: 'create', component: CreateEventComponent}
    ]),
  ]
})
export class HostModule
{
}
