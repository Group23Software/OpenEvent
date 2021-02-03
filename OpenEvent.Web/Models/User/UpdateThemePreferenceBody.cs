using System;

namespace OpenEvent.Web.Models.User
{
    public class UpdateThemePreferenceBody
    {
        public bool IsDarkMode { get; set; }
        public Guid Id { get; set; }
    }
}