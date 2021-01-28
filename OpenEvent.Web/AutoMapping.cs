using System.Text;
using AutoMapper;
using OpenEvent.Web.Models;

namespace OpenEvent.Web
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(d => d.Avatar, m => m.MapFrom(x => Encoding.UTF8.GetString(x.Avatar, 0, x.Avatar.Length)));
        }
    }
}