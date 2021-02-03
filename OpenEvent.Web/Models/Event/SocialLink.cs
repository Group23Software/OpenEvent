using System;
using Microsoft.EntityFrameworkCore;

namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Social link for event.
    /// </summary>
    [Owned]
    public class SocialLink
    {
        public Guid Id { get; set; }
        public SocialMedia SocialMedia { get; set; }
        public string Link { get; set; }
    }
}