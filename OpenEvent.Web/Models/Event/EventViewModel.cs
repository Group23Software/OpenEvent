using System;
using System.Collections.Generic;
using OpenEvent.Web.Models.Category;

namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Minimum data representing an event.
    /// </summary>
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Thumbnail { get; set; }
        public bool IsOnline { get; set; }
        
        public DateTime StartLocal { get; set; }
        public DateTime StartUTC { get; set; }
        public DateTime EndLocal { get; set; }
        public DateTime EndUTC { get; set; }
        public decimal Price { get; set; }
        
        public List<CategoryViewModel> Categories { get; set; }
    }
}