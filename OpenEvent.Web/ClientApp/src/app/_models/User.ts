import {TicketViewModel} from "./Ticket";
import {PaymentMethodViewModel} from "./PaymentMethod";
import {Address} from "./Address";
import {BankAccountViewModel} from "./BankAccount";

export interface UserViewModel
{
  Id: string;
  UserName: string;
  Avatar: string;
  Token?: string;
  IsDarkMode: boolean;
}

interface StripeAccountInfo
{
  PayoutsEnabled: boolean;
  ChargesEnabled: boolean;
  DefaultCurrency: boolean;
  Requirements: any;
  PersonId: string;
}

export interface UserAccountModel extends UserViewModel
{
  // Id: string;
  // UserName: string;
  Email?: string;
  FirstName?: string;
  LastName?: string;
  // Avatar: string;
  PhoneNumber?: string;
  DateOfBirth?: Date;
  // IsDarkMode: boolean;
  Tickets?: TicketViewModel[];
  PaymentMethods?: PaymentMethodViewModel[];
  BankAccounts?: BankAccountViewModel[];
  Address?: Address;
  StripeAccountId?: string;
  StripeCustomerId?: string;
  StripeAccountInfo?: StripeAccountInfo;
}

export interface AuthBody
{
  Email: string;
  Password: string;
  Remember: boolean;
}

export interface UpdatePasswordBody
{
  Email: string;
  Password: string;
}

export interface NewUserInput
{
  UserName: string;
  Password: string;
  Email: string;
  FirstName: string;
  LastName: string;
  Avatar: any[];
  PhoneNumber: string;
  DateOfBirth: Date;
  Remember: boolean;
}

export interface UpdateUserNameBody
{
  Id: string;
  UserName: string;
}

export interface UpdateAvatarBody
{
  Id: string;
  Avatar: any[];
}

export interface UpdateThemePreferenceBody
{
  Id: string;
  IsDarkMode: boolean;
}

export interface UpdateUserAddressBody
{
  Id: string;
  Address: Address;
}
