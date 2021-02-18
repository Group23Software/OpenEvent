export interface BankAccountViewModel
{
  StripeBankAccountId: string;
  Name: string;
  Country: string;
  Currency: string;
  LastFour: string;
  Bank: string;
}

export interface AddBankAccountBody
{
  UserId: string;
  BankToken: string;
}

export interface RemoveBankAccountBody
{
  UserId: string;
  BankId: string;
}
