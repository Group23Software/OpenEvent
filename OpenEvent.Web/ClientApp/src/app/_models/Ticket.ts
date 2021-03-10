import {EventViewModel} from "./Event";
import {TransactionViewModel} from "./Transaction";

export interface TicketViewModel
{
  Id: string;
  QRCode: string;
  EventId: string;
  EventName: string;
  EventStart: Date;
  EventEnd: Date;
}

export interface TicketDetailModel
{
  Id: string;
  QRCode: string;
  Event: EventViewModel;
  Transaction: TransactionViewModel;
}

export interface VerifyTicketBody
{
  Id: string;
  EventId: string;
}
