using System;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    /// Request body for updating user's avatar.
    /// </summary>
    public class UpdateAvatarBody
    {
        public Guid Id { get; set; }
        public byte[] Avatar { get; set; }
    }
}