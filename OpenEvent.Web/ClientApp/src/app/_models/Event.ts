import {Category, CategoryViewModel} from "./Category";
import {Address} from "./Address";
import {SocialLinkViewModel} from "./SocialLink";
import {ImageViewModel} from "./Image";
import {TicketViewModel} from "./Ticket";
import {TransactionViewModel} from "./Transaction";
import {Promo} from "./Promo";

export interface EventDetailModel extends EventViewModel
{
  Images: ImageViewModel[];
  SocialLinks: SocialLinkViewModel[];
  Address: Address;
  TicketsLeft: number;
  // Tickets: TicketViewModel[];
}

export interface EventHostModel extends EventViewModel
{
  Created: Date;
  Images: ImageViewModel[];
  SocialLinks: SocialLinkViewModel[];
  Address: Address;
  TicketsLeft: number;
  Tickets: TicketViewModel[];
  Transactions: TransactionViewModel[];
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
  Finished?: boolean;

  Categories: CategoryViewModel[];
  Promos: Promo[];
}

export interface PopularEventViewModel extends EventViewModel
{
  Score: number;
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
  Categories: Category[];
}

export interface UpdateEventBody
{
  Id: string;
  Name: string;
  Description: string;
  Thumbnail: ImageViewModel;
  Images: ImageViewModel[];
  SocialLinks: SocialLinkViewModel[];
  StartLocal: Date;
  EndLocal: Date;
  Price: number;
  Address: Address;
  IsOnline: boolean;
  Categories: Category[];
  Finished: boolean;
}

export enum SearchParam
{
  Category,
  Location,
  IsOnline,
  Date
}

export interface SearchFilter {
  Key: SearchParam;
  Value: string;
}
