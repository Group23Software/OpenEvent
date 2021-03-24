using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace OpenEvent.Web.UserOwnsEvent
{
    /// <summary>
    /// Attribute applied to a controller method if user owns check is needed
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UserOwnsEventAttribute : Attribute, IFilterFactory
    {
        /// <inheritdoc />
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<UserOwnsEventFilter>();
        }

        /// <inheritdoc />
        public bool IsReusable => false;
    }
}