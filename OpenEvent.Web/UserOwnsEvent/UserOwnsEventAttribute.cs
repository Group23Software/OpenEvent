using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace OpenEvent.Web.UserOwnsEvent
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UserOwnsEventAttribute : Attribute, IFilterFactory
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<UserOwnsEventFilter>();
        }

        public bool IsReusable => false;
    }
}