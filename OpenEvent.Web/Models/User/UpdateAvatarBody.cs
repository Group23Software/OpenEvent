using System;

namespace OpenEvent.Web.Models.User
{
    public class UpdateAvatarBody
    {
        public Guid Id { get; set; }
        public byte[] Avatar { get; set; }
    }
}