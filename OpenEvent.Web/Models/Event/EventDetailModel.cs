using System;
using System.Collections.Generic;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Promo;

namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Event data needed for displaying an event page.
    /// </summary>
    public class EventDetailModel
    {
        /// <summary>
        /// Event's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of event
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
        /// Start of event
        /// </summary>
        public DateTime StartLocal { get; set; }

        /// <summary>
        /// Start of event
        /// </summary>
        public DateTime StartUTC { get; set; }

        /// <summary>
        /// End of event
        /// </summary>
        public DateTime EndLocal { get; set; }

        /// <summary>
        /// End of event
        /// </summary>
        public DateTime EndUTC { get; set; }

        /// <summary>
        /// Price of event as long eg: £10.50 = 1050
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Physical address of event including coordinates
        /// </summary>
        public Address.Address Address { get; set; }

        /// <summary>
        /// If the event is online
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Number of tickets that are still available for purchase
        /// </summary>
        public int TicketsLeft { get; set; }

        /// <summary>
        /// List of categories the event has
        /// </summary>
        public List<CategoryViewModel> Categories { get; set; }

        /// <summary>
        /// All promos currently active
        /// </summary>
        public List<PromoViewModel> Promos { get; set; }

        /// <summary>
        /// If the event has finished
        /// </summary>
        public bool Finished { get; set; }
    }
}