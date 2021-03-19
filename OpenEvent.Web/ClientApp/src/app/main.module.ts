import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from "./shared.module";
import {RouterModule} from "@angular/router";


@NgModule({
  declarations: [
    // SearchComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      // {path: 'main', redirectTo: 'search', pathMatch: 'full'},
      // {
      //   path: 'search', component: SearchComponent, canActivate: [AuthGuard]
      // }
    ])
  ]
})
export class MainModule
{
}
