using System.Text;
using AutoMapper;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(d => d.Avatar, m => m.MapFrom(x => Encoding.UTF8.GetString(x.Avatar, 0, x.Avatar.Length)));
            CreateMap<User, UserAccountModel>()
                .ForMember(d => d.Avatar, m => m.MapFrom(x => Encoding.UTF8.GetString(x.Avatar, 0, x.Avatar.Length)));
            CreateMap<Ticket, TicketViewModel>().ForMember(d => d.QRCode,
                m => m.MapFrom(x => Encoding.UTF8.GetString(x.QRCode, 0, x.QRCode.Length)));
            CreateMap<Image, ImageViewModel>().ForMember(d => d.Source,
                m => m.MapFrom(x => Encoding.UTF8.GetString(x.Source, 0, x.Source.Length)));
            CreateMap<Category, CategoryViewModel>();
        }
    }
}