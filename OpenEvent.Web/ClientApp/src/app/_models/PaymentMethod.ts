export interface AddPaymentMethodBody
{
  UserId: string;
  CardToken: string;
  NickName: string;
}

export interface RemovePaymentMethodBody
{
  UserId: string;
  PaymentId: string;
}

export interface MakeDefaultBody
{
  UserId: string;
  PaymentId: string;
}

export interface PaymentMethodViewModel
{
  StripeCardId: string;
  Name: string;
  Brand: string;
  Funding: string;
  ExpiryMonth: number;
  ExpiryYear: number;
  LastFour: string;
  Country: string;
  IsDefault: boolean;
  NickName: string;
}
