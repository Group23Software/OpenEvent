import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from "./shared.module";
import {RouterModule} from "@angular/router";
import {AuthGuard} from "./_guards/auth.guard";
import {MyTicketsComponent} from "./my-tickets/my-tickets.component";
import { TicketComponent } from './ticket/ticket.component';
import { TicketReceiptDialogComponent } from './ticket/ticket-receipt-dialog/ticket-receipt-dialog.component';


@NgModule({
  declarations: [
    MyTicketsComponent,
    TicketComponent,
    TicketReceiptDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: 'account',
        loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
        canActivate: [AuthGuard]
      },
      {path: 'tickets', component: MyTicketsComponent},
      {path: 'ticket/:id', component: TicketComponent}
    ])
  ]
})
export class UserModule
{

}
