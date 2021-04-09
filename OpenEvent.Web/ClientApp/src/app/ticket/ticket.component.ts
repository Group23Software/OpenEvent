import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {TicketService} from "../_Services/ticket.service";
import {TicketDetailModel} from "../_models/Ticket";
import {HttpErrorResponse} from "@angular/common/http";
import {MatDialog} from "@angular/material/dialog";
import {TicketReceiptDialogComponent} from "./ticket-receipt-dialog/ticket-receipt-dialog.component";

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css']
})
export class TicketComponent implements OnInit
{
  public Ticket: TicketDetailModel;
  public getTicketError: string;
  public loading: boolean = true;

  constructor (private route: ActivatedRoute, private ticketService: TicketService, private dialog: MatDialog)
  {
  }

  ngOnInit (): void
  {
    const id = this.route.snapshot.paramMap.get('id');
    this.ticketService.Get(id).subscribe(t =>
    {
      this.Ticket = t;
    }, (e: HttpErrorResponse) =>
    {
      this.getTicketError = e.error.Message
    }, () => this.loading = false);
  }

  openReceipt ()
  {
    this.dialog.open(TicketReceiptDialogComponent, {
      data: {
        Ticket: this.Ticket
      },
      width: "80vw"
    })
  }
}
