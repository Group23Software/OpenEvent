using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        public DateTime StartLocal { get; set; }
        public DateTime StartUTC { get; set; }
        public DateTime EndLocal { get; set; }
        public DateTime EndUTC { get; set; }
        public decimal Price { get; set; }
        public User.User Host { get; set; }
        public Address Address { get; set; }
        public bool IsOnline { get; set; }
        public List<Ticket.Ticket> Tickets { get; set; }
        [NotMapped] public int TicketsLeft => Tickets.Select(x => x == null).Count();
        public List<EventCategory> EventCategories { get; set; }
        public bool isCanceled { get; set; }
    }
}