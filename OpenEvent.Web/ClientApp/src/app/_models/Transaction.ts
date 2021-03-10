export interface CreateIntentBody
{
  UserId: string;
  EventId: string;
// public int NumberOfTickets { get; set; }
  Amount: number;
}

export interface TransactionViewModel
{
  EventId: string;
  StripeIntentId: string;
  Start: Date;
  Updated: Date;
  End: Date;
  Amount: number;
  Paid: boolean;
  Status: string;
  TicketId: string;
  NextAction: any;
  ClientSecret?: string;
  PromoId: string;
}

export interface InjectPaymentMethodBody
{
  UserId: string;
  IntentId: string;
  CardId: string;
}

export interface ConfirmIntentBody
{
  UserId: string;
  IntentId: string;
}

export interface CancelIntentBody {
  Id: string;
  EventId: string;
  TicketId: string;
}
