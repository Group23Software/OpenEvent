import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {TicketService} from "../_Services/ticket.service";
import {TicketDetailModel} from "../_models/Ticket";
import {HttpErrorResponse} from "@angular/common/http";

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

  constructor (private route: ActivatedRoute, private ticketService: TicketService)
  {
  }

  ngOnInit (): void
  {
    const id = this.route.snapshot.paramMap.get('id');
    this.ticketService.Get(id).subscribe(t =>
    {
      this.Ticket = t;
      console.log(this.Ticket);
    }, (e: HttpErrorResponse) =>
    {
      this.getTicketError = e.error.Message
    }, () => this.loading = false);
  }

}
