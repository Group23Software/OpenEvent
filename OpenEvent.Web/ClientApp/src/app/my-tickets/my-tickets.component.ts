import {Component, OnInit} from '@angular/core';
import {TicketService} from "../_Services/ticket.service";
import {TicketViewModel} from "../_models/Ticket";
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-my-tickets',
  templateUrl: './my-tickets.component.html',
  styleUrls: ['./my-tickets.component.css']
})
export class MyTicketsComponent implements OnInit
{
  public gettingTicketsError: string;
  public loading: boolean = true;
  public Tickets: TicketViewModel[];

  constructor (private ticketService: TicketService, private router: Router)
  {
  }

  ngOnInit (): void
  {
    this.ticketService.GetAllUsersTickets().subscribe(tickets =>
    {
      this.Tickets = tickets;
    }, (e: HttpErrorResponse) =>
    {
      this.gettingTicketsError = e.error.Message;
    }, () => this.loading = false);
  }

  openTicket (ticket: TicketViewModel)
  {
    this.router.navigate(['/user/ticket', ticket.Id]);
  }
}
