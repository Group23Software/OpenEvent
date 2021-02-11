import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from "./shared.module";
import {RouterModule} from "@angular/router";
import {AuthGuard} from "./_guards/auth.guard";
import {SearchComponent} from './search/search.component';


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: 'account',
        loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
        canActivate: [AuthGuard]
      }
    ])
  ]
})
export class UserModule
{

}
