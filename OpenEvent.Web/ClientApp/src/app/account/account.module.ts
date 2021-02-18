import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";
import {AccountComponent} from "./account.component";
import {SharedModule} from "../shared.module";
import {UserModule} from "../user.module";
import { AccountPreferencesComponent } from './account-preferences/account-preferences.component';
import { PaymentMethodsComponent } from './payment-methods/payment-methods.component';
import {NgxStripeModule} from "ngx-stripe";
import { VirtualCardComponent } from './payment-methods/virtual-card/virtual-card.component';


@NgModule({
  declarations: [
    AccountComponent,
    AccountPreferencesComponent,
    PaymentMethodsComponent,
    VirtualCardComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    UserModule,
    RouterModule.forChild([
      {path: '', component: AccountComponent, pathMatch: 'full'}
    ]),
    NgxStripeModule,
  ]
})
export class AccountModule
{
}
