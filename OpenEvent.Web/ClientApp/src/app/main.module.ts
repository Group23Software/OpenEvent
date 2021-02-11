import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from "./shared.module";
import {RouterModule} from "@angular/router";
import {SearchComponent} from "./search/search.component";
import {AuthGuard} from "./_guards/auth.guard";


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
