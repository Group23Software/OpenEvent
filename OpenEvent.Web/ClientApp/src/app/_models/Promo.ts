export interface Promo
{
  Id: string;
  Start: Date;
  End: Date;
  Active: boolean;
  Discount: number;
}

export interface CreatePromoBody
{
  Start: Date;
  End: Date;
  Active: boolean;
  Discount: number;
  EventId: string;
}

export interface UpdatePromoBody
{
  Id: string;
  Start: Date;
  End: Date;
  Active: boolean;
  Discount: number;
}
