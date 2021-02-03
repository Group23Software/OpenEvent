using System;
using Microsoft.EntityFrameworkCore;

namespace OpenEvent.Web.Models.Event
{
    /// <summary>
    /// Image as bitmap with label.
    /// </summary>
    [Owned]
    public class Image
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public byte[] Source { get; set; }
    }
}