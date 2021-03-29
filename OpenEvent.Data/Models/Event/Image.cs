using System;
using Microsoft.EntityFrameworkCore;

namespace OpenEvent.Data.Models.Event
{
    /// <summary>
    /// Image as bitmap with label.
    /// </summary>
    [Owned]
    public class Image
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Image label/caption
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Bitmap encoded into byte array
        /// </summary>
        public byte[] Source { get; set; }
    }
}