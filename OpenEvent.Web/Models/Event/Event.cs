using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.Category;

namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Base event model with all data.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public Guid Id { get; set; }

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
        public Image Thumbnail { get; set; }

        /// <summary>
        /// Event image gallery
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// Social media links
        /// </summary>
        public List<SocialLink> SocialLinks { get; set; }

        /// <summary>
        /// Date time of creation
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Starting time
        /// </summary>
        public DateTime StartLocal { get; set; }

        /// <summary>
        /// Universal starting time
        /// </summary>
        public DateTime StartUTC { get; set; }

        /// <summary>
        /// Ending time
        /// </summary>
        public DateTime EndLocal { get; set; }

        /// <summary>
        /// Universal ending time 
        /// </summary>
        public DateTime EndUTC { get; set; }

        /// <summary>
        /// base price of ticket saved as long, eg: £10.50 as 1050
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// User who is hosting the event
        /// </summary>
        [JsonIgnore]
        public User.User Host { get; set; }

        /// <summary>
        /// Address if the event has one
        /// </summary>
        public Address.Address Address { get; set; }

        /// <summary>
        /// If the event is online
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// List of all tickets associated with the event
        /// </summary>
        public List<Ticket.Ticket> Tickets { get; set; }

        /// <summary>
        /// Number of tickets not sold yet
        /// </summary>
        public int TicketsLeft { get; set; }

        /// <summary>
        /// List of event categories
        /// </summary>
        public List<EventCategory> EventCategories { get; set; }

        /// <summary>
        /// If the event has been canceled
        /// </summary>
        public bool isCanceled { get; set; }

        /// <summary>
        /// List of page views
        /// </summary>
        [JsonIgnore]
        public List<PageViewEvent> PageViewEvents { get; set; }

        /// <summary>
        /// List of ticket verifications
        /// </summary>
        [JsonIgnore]
        public List<TicketVerificationEvent> VerificationEvents { get; set; }

        /// <summary>
        /// List of all transactions associated with the event
        /// </summary>
        public List<Transaction.Transaction> Transactions { get; set; }

        /// <summary>
        /// List of event's promos
        /// </summary>
        public List<Promo.Promo> Promos { get; set; }

        /// <summary>
        /// If the event has finished
        /// </summary>
        public bool Finished { get; set; }
    }
}