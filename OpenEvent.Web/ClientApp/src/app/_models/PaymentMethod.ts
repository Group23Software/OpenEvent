export interface AddPaymentMethodModel
{
  UserId: string;
  CardToken: string;
  NickName: string;
}

export interface PaymentMethodViewModel
{
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
