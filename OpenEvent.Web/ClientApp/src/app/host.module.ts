import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateEventComponent } from './event/create-event/create-event.component';
import {RouterModule} from "@angular/router";
import {SharedModule} from "./shared.module";



@NgModule({
  declarations: [CreateEventComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {path: 'create', component: CreateEventComponent}
    ]),
  ]
})
export class HostModule { }
