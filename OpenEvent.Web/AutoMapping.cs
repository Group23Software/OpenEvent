using System;
using System.Linq;
using System.Text;
using AutoMapper;
using OpenEvent.Data.Models.Analytic;
using OpenEvent.Data.Models.BankAccount;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.PaymentMethod;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Data.Models.Recommendation;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Data.Models.Transaction;
using OpenEvent.Data.Models.User;

namespace OpenEvent.Web
{
    /// <summary>
    /// Automapping profile, defines all maps
    /// </summary>
    public class AutoMapping : Profile
    {
        /// <inheritdoc />
        public AutoMapping()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(d => d.Avatar, m => m.MapFrom(x => Encoding.UTF8.GetString(x.Avatar, 0, x.Avatar.Length)));

            CreateMap<Ticket, TicketViewModel>()
                .ForMember(d => d.QRCode, m => m.MapFrom(x => Encoding.UTF8.GetString(x.QRCode, 0, x.QRCode.Length)))
                .ForMember(x => x.EventName, m => m.MapFrom(x => x.Event.Name))
                .ForMember(x => x.EventStart, m => m.MapFrom(x => x.Event.StartLocal))
                .ForMember(x => x.EventEnd, m => m.MapFrom(x => x.Event.EndLocal))
                .ForMember(x => x.EventId, m => m.MapFrom(x => x.Event.Id));

            CreateMap<Image, ImageViewModel>()
                .ForMember(d => d.Source, m => m.MapFrom(x => Encoding.UTF8.GetString(x.Source, 0, x.Source.Length)));

            CreateMap<Category, CategoryViewModel>();

            CreateMap<SocialLink, SocialLinkViewModel>();

            CreateMap<ImageViewModel, Image>()
                .ForMember(d => d.Source, m => m.MapFrom(x => Encoding.UTF8.GetBytes(x.Source)))
                .ForMember(x => x.Id, op => op.Ignore());

            CreateMap<Event, EventViewModel>()
                .ForMember(x => x.Categories, op => op.Ignore())
                .ForMember(x => x.Promos, op => op.MapFrom(x => x.Promos.Where(x => x.Active && x.Start < DateTime.Now && DateTime.Now < x.End).ToList()));

            CreateMap<SocialLinkViewModel, SocialLink>()
                .ForMember(x => x.Id, op => op.Ignore());

            CreateMap<Ticket, TicketDetailModel>()
                .ForMember(d => d.QRCode, m => m.MapFrom(x => Encoding.UTF8.GetString(x.QRCode, 0, x.QRCode.Length)));

            CreateMap<PaymentMethod, PaymentMethodViewModel>();
            
            CreateMap<BankAccount, BankAccountViewModel>();
            
            CreateMap<Stripe.Account, StripeAccountInfo>().ForMember(x => x.PersonId, m => m.MapFrom(x => x.Individual.Id));

            CreateMap<SearchEvent, SearchEventViewModel>();
            
            CreateMap<PageViewEvent, PageViewEventViewModel>();
            
            CreateMap<Transaction, TransactionViewModel>();
            
            CreateMap<RecommendationScore, RecommendationScoreViewModel>();
            
            CreateMap<TicketVerificationEvent, TicketVerificationEventViewModel>();
            
            CreateMap<Promo, PromoViewModel>().ForMember(x => x.NumberOfSales, op => op.Ignore());
            
            CreateMap<Event, PopularEventViewModel>()
                .ForMember(x => x.Categories, op => op.Ignore())
                .ForMember(x => x.Score, op => op.Ignore());
        }
    }
}