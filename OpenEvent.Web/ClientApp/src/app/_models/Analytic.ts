// public class UsersAnalytics
// {
//   public List<PageViewEvent> PageViewEvents { get; set; }
// public List<SearchEvent> SearchEvents { get; set; }
// }

export interface UsersAnalytics
{
  PageViewEvents: PageViewEvent[];
  SearchEvents: SearchEvent[];
  RecommendationScores: RecommendationScore[];
  TicketVerificationEvents: TicketVerificationEvent[];
}

export interface MappedUsersAnalytics
{
  PageViewEvents: PageViewByDate[];
  SearchEvents: SearchEventByDate[];
  RecommendationScores: RecommendationScore[];
  TicketVerificationEvents: TicketVerificationEvent[];
}

export interface EventAnalytics
{
  PageViewEvents: PageViewEvent[];
  TicketVerificationEvents: TicketVerificationEvent[],
  AverageRecommendationScores: AverageRecommendationScore[]
}

export interface MappedEventAnalytics
{
  PageViewEvents: PageViewByDate[];
  TicketVerificationEvents: TicketVerificationEvent[];
  AverageRecommendationScores: AverageRecommendationScore[]
}

export interface AverageRecommendationScore
{
  CategoryName: string;
  Weight: number;
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

export interface TicketVerificationEvent extends AnalyticEvent
{
  UserId: string;
  TicketId: string;
  EventId: string;
}

export interface RecommendationScore
{
  Id: string;
  CategoryName: string;
  Weight: number;
}

export interface PageViewByDate
{
  Date: Date;
  PageViews: PageViewEvent[];
}

export interface SearchEventByDate
{
  Date: Date;
  Searches: SearchEvent[];
}
