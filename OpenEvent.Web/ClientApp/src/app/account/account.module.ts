import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";
import {AccountComponent} from "./account.component";
import {SharedModule} from "../shared.module";
import {UserModule} from "../user.module";
import { AccountPreferencesComponent } from './account-preferences/account-preferences.component';


@NgModule({
  declarations: [
    AccountComponent,
    AccountPreferencesComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    UserModule,
    RouterModule.forChild([
      {path: '', component: AccountComponent, pathMatch: 'full'}
    ]),
  ]
})
export class AccountModule
{
}
