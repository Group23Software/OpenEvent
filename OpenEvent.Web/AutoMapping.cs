using AutoMapper;
using OpenEvent.Web.Models;

namespace OpenEvent.Web
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserViewModel>();
        }
    }
}