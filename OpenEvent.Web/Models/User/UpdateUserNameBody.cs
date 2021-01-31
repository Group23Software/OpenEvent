using System;

namespace OpenEvent.Web.Models.User
{
    public class UpdateUserNameBody
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}