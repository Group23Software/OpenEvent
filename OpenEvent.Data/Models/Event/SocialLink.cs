using System;
using Microsoft.EntityFrameworkCore;

namespace OpenEvent.Data.Models.Event
{
    /// <summary>
    /// Social link for event.
    /// </summary>
    [Owned]
    public class SocialLink
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Valid social media
        /// </summary>
        public SocialMedia SocialMedia { get; set; }

        /// <summary>
        /// url
        /// </summary>
        public string Link { get; set; }
    }
}