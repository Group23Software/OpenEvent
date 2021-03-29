using System;
using System.Collections.Generic;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Data.Models.Transaction;

namespace OpenEvent.Data.Models.Event
{
    // TODO: docs
    /// <summary>
    /// Event model for host view (including tickets and transaction).
    /// </summary>
    public class EventHostModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ImageViewModel Thumbnail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewModel> Images { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SocialLinkViewModel> SocialLinks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartLocal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartUTC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndLocal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndUTC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Address.Address Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TicketViewModel> Tickets { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TransactionViewModel> Transactions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TicketsLeft { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CategoryViewModel> Categories { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PromoViewModel> Promos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Finished { get; set; }
    }
}