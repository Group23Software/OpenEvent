import {TicketViewModel} from "./Ticket";

export interface UserViewModel
{
  Id: string;
  UserName: string;
  Avatar: string;
  Token: string;
  IsDarkMode: boolean;
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
