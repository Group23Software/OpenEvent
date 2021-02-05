import {Category, CategoryViewModel} from "./Category";
import {Address} from "./Address";
import {SocialLinkViewModel} from "./SocialLink";
import {ImageViewModel} from "./Image";
import {TicketViewModel} from "./Ticket";

export interface EventDetailModel extends EventViewModel
{
  Images: ImageViewModel[];
  SocialLinks: SocialLinkViewModel[];
  Address: Address;
  TicketsLeft: number;
}

export interface EventHostModel
{
  Images: ImageViewModel[];
  SocialLinks: SocialLinkViewModel[];
  Address: Address;
  TicketsLeft: number;
  Tickets: TicketViewModel[];

  //TODO: transactions
}

export interface EventViewModel
{
  Id: string;
  Name: string;
  Description: string;
  Thumbnail: ImageViewModel;
  IsOnline: boolean;
  StartLocal: Date;
  StartUTC: Date;
  EndLocal: Date;
  EndUTC: Date;
  Price: number;

  Categories: CategoryViewModel[];
}

export interface CreateEventBody
{
  Name: string;
  Description: string;
  Thumbnail: ImageViewModel;
  Images: ImageViewModel[];
  SocialLinks: SocialLinkViewModel[];
  StartLocal: Date;
  EndLocal: Date;
  Price: number;
  HostId: string;
  Address: Address;
  IsOnline: boolean;
  NumberOfTickets: number;
  Categories: Category;
}
