using System;

namespace OpenEvent.Data.Models.User
{
    /// <summary>
    /// Request body for updating user's avatar.
    /// </summary>
    public class UpdateAvatarBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Avatar bitmap image as byte array
        /// </summary>
        public byte[] Avatar { get; set; }
    }
}