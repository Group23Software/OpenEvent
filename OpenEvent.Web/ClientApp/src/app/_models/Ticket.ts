import {EventViewModel} from "./Event";

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
}

export interface VerifyTicketBody
{
  Id: string;
  EventId: string;
}
