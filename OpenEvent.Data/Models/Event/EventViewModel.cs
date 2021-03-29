using System;
using System.Collections.Generic;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Promo;

namespace OpenEvent.Data.Models.Event
{
    /// <summary>
    /// Minimum data representing an event.
    /// </summary>
    public class EventViewModel
    {
        /// <summary>
        /// Event's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Event's name
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
        /// If the event is online
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// start of event
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
        /// List of categories
        /// </summary>
        public List<CategoryViewModel> Categories { get; set; }

        /// <summary>
        /// List of current promos
        /// </summary>
        public List<PromoViewModel> Promos { get; set; }

        /// <summary>
        /// If the event is finished
        /// </summary>
        public bool Finished { get; set; }
    }
}