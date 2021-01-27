export interface UserViewModel
{
  Id: string;
  UserName: string;
  Email: string;
  FirstName: string;
  LastName: string;
  Avatar: any[];
  PhoneNumber: string;
  Token: string;
}

export interface AuthBody
{
  Email: string;
  Password: string;
  Remember: boolean;
}
