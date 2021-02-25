// public class UsersAnalytics
// {
//   public List<PageViewEvent> PageViewEvents { get; set; }
// public List<SearchEvent> SearchEvents { get; set; }
// }

export interface UsersAnalytics
{
  PageViewEvents: PageViewEvent[];
  SearchEvents: SearchEvent[];
}

export interface AnalyticEvent
{
  Id: string;
  Created: Date;
}

export interface PageViewEvent extends AnalyticEvent
{
  EventId: string;
}

export interface SearchEvent extends AnalyticEvent
{
  Search: string;
  Params: string;
}
