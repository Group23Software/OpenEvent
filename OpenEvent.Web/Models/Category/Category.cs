using System;
using System.Collections.Generic;

namespace OpenEvent.Web.Models.Category
{
    /// <summary>
    /// Category for event.
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<EventCategory> Events { get; set; }
    }
}