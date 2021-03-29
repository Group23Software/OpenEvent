namespace OpenEvent.Data.Models.Event
{
    /// <summary>
    /// Image view model
    /// </summary>
    public class ImageViewModel
    {
        /// <summary>
        /// Image label/caption
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Bitmap image encoded into string
        /// </summary>
        public string Source { get; set; }
    }
}