import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {TicketDetailModel} from "../../_models/Ticket";
import {TicketService} from "../../_Services/ticket.service";

@Component({
  selector: 'app-ticket-receipt-dialog',
  templateUrl: './ticket-receipt-dialog.component.html',
  styleUrls: ['./ticket-receipt-dialog.component.css']
})
export class TicketReceiptDialogComponent implements OnInit
{

  Ticket: TicketDetailModel;

  constructor (@Inject(MAT_DIALOG_DATA) public data: {Ticket: TicketDetailModel}, private ticketService: TicketService)
  {
    this.Ticket = data.Ticket;
  }

  ngOnInit (): void
  {
    this.ticketService.GetCharge("ch_1ITQNEK2ugLXrgQX0uLfbPE9").subscribe(x => console.log(x));
  }

}
