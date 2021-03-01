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
import { BankAccountComponent } from './bank-account/bank-account.component';
import { UserDataComponent } from './user-data/user-data.component';


@NgModule({
    declarations: [
        AccountComponent,
        AccountPreferencesComponent,
        PaymentMethodsComponent,
        BankAccountComponent,
        UserDataComponent,
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
