import {CategoryViewModel} from "./Category";
import {TicketViewModel} from "./Ticket";
import {Address} from "./Address";
import {SocialLinkViewModel} from "./SocialLink";
import {ImageViewModel} from "./Image";

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
