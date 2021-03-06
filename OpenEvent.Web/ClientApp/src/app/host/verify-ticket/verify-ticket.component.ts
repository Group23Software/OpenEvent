import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {BarcodeFormat, Exception} from "@zxing/library";
import {FormControl} from "@angular/forms";
import {TicketService} from "../../_Services/ticket.service";
import {HttpErrorResponse} from "@angular/common/http";
import {MAT_DIALOG_DATA, MatDialog} from "@angular/material/dialog";
import {TriggerService} from "../../_Services/trigger.service";
import {IteratorStatus} from "../../_extensions/iterator/iterator.component";

@Component({
  template: '<mat-error>Ticket not valid!</mat-error><mat-dialog-actions align="end"><button mat-button color="warn" mat-dialog-close>Close</button></mat-dialog-actions>'
})
export class VerifyDialog
{
  constructor (@Inject(MAT_DIALOG_DATA) public data: any)
  {
  }
}

@Component({
  selector: 'app-verify-ticket',
  templateUrl: './verify-ticket.component.html',
  styleUrls: ['./verify-ticket.component.css']
})
export class VerifyTicketComponent implements OnInit
{

  public Id: FormControl = new FormControl();
  private EventId: string;
  public allowedFormats = [BarcodeFormat.QR_CODE];
  public loading: boolean;
  public verifyError: string;

  private lastTicket: string;

  constructor (private route: ActivatedRoute, private ticketService: TicketService, private trigger: TriggerService, private dialog: MatDialog)
  {
  }

  ngOnInit (): void
  {
    this.EventId = this.route.snapshot.paramMap.get('id');
  }

  public Verify ()
  {
    this.ticketService.Verify({
      Id: this.Id.value,
      EventId: this.EventId
    }).subscribe(r =>
    {
      this.trigger.IterateForever('Verified Ticket', IteratorStatus.good);
    }, (e: HttpErrorResponse) =>
    {
      const dialogRef = this.dialog.open(VerifyDialog);
      dialogRef.afterClosed().subscribe(x => this.loading = false)
    }, () => this.loading = false);
  }

  public scanSuccess (ticketId: string)
  {
    if (ticketId != this.lastTicket)
    {
      this.lastTicket = ticketId;
      this.loading = true;

      this.ticketService.Verify({
        Id: ticketId,
        EventId: this.EventId
      }).subscribe(r =>
      {
        this.trigger.IterateForever('Verified Ticket', IteratorStatus.good);
      }, (e: HttpErrorResponse) =>
      {
        const dialogRef = this.dialog.open(VerifyDialog);
        dialogRef.afterClosed().subscribe(x => this.loading = false)
      }, () => this.loading = false);
    }
  }

  scanFailure (event: Exception)
  {

  }

  scanError (event: Error)
  {

  }
}
