using System;
using System.Collections.Generic;

namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Request body for updating an event
    /// </summary>
    public class UpdateEventBody
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
        /// End of event
        /// </summary>
        public DateTime EndLocal { get; set; }

        /// <summary>
        /// Price of event as long eg: £10.50 = 1050
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Physical address of event including coordinates
        /// </summary>
        public Address.Address Address { get; set; }

        /// <summary>
        /// If the event is online
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// List of categories the event has
        /// </summary>
        public List<Category.Category> Categories { get; set; }

        /// <summary>
        /// If the event is finished
        /// </summary>
        public bool Finished { get; set; }
    }
}