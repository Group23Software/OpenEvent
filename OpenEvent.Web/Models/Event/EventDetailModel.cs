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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ImageViewModel Thumbnail { get; set; }
        public List<ImageViewModel> Images { get; set; }
        public List<SocialLinkViewModel> SocialLinks { get; set; }
        public DateTime StartLocal { get; set; }
        public DateTime StartUTC { get; set; }
        public DateTime EndLocal { get; set; }
        public DateTime EndUTC { get; set; }
        public decimal Price { get; set; }
        public Address.Address Address { get; set; }
        public bool IsOnline { get; set; }
        public int TicketsLeft { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<PromoViewModel> Promos { get; set; }
    }
}