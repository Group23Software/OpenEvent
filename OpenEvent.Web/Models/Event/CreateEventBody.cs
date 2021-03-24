using System;
using System.Collections.Generic;

namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Request body for creating an event
    /// </summary>
    public class CreateEventBody
    {
        /// <summary>
        /// Event name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Markdown description parsed to sting
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Event thumbnail
        /// </summary>
        public ImageViewModel Thumbnail { get; set; }

        /// <summary>
        /// Event image gallery
        /// </summary>
        public List<ImageViewModel> Images { get; set; }

        /// <summary>
        /// Social media links
        /// </summary>
        public List<SocialLinkViewModel> SocialLinks { get; set; }

        /// <summary>
        /// Starting date time
        /// </summary>
        public DateTime StartLocal { get; set; }

        /// <summary>
        /// Ending date time
        /// </summary>
        public DateTime EndLocal { get; set; }

        /// <summary>
        /// base price of ticket saved as long, eg: £10.50 as 1050
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Id of the host user
        /// </summary>
        public Guid HostId { get; set; }

        /// <summary>
        /// Events address
        /// </summary>
        public Address.Address Address { get; set; }

        /// <summary>
        /// If the event is online
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Number of tickets to generate
        /// </summary>
        public int NumberOfTickets { get; set; }

        /// <summary>
        /// Categories to add
        /// </summary>
        public List<Category.Category> Categories { get; set; }
    }
}