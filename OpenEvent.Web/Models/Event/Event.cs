using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.Category;

namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Full event model with all data.
    /// </summary>
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Thumbnail { get; set; }
        public List<Image> Images { get; set; }
        public List<SocialLink> SocialLinks { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartLocal { get; set; }
        public DateTime StartUTC { get; set; }
        public DateTime EndLocal { get; set; }
        public DateTime EndUTC { get; set; }
        public long Price { get; set; }
        [JsonIgnore] public User.User Host { get; set; }
        public Address.Address Address { get; set; }
        public bool IsOnline { get; set; }
        public List<Ticket.Ticket> Tickets { get; set; }
        public int TicketsLeft { get; set; }
        public List<EventCategory> EventCategories { get; set; }
        public bool isCanceled { get; set; }

        [JsonIgnore] public List<PageViewEvent> PageViewEvents { get; set; }
        [JsonIgnore] public List<TicketVerificationEvent> VerificationEvents { get; set; }
        public List<Transaction.Transaction> Transactions { get; set; } 
        public List<Promo.Promo> Promos { get; set; }
        public bool Finished { get; set; }
    }
}