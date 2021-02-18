using System;
using System.Collections.Generic;

namespace OpenEvent.Web.Models.Event
{
    public class UpdateEventBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ImageViewModel Thumbnail { get; set; }
        public List<ImageViewModel> Images { get; set; }
        public List<SocialLinkViewModel> SocialLinks { get; set; }
        public DateTime StartLocal { get; set; }
        public DateTime EndLocal { get; set; }
        public decimal Price { get; set; }
        public Address.Address Address { get; set; }
        public bool IsOnline { get; set; }
        public List<Category.Category> Categories { get; set; }
    }
}