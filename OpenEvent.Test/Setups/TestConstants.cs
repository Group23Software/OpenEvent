using System;
using System.Collections.Generic;
using OpenEvent.Web.Models.Category;

namespace OpenEvent.Test.Setups
{
    public static class TestConstants
    {
        public static List<Category> Categories = new List<Category>
        {
            // , Id = new Guid("534DE110-2D1D-4AE8-9293-68FC8037DB5A")
            new() {Name = "Music"},
            new() {Name = "Business"},
            new() {Name = "Charity"},
            new() {Name = "Culture"},
            new() {Name = "Family"},
            new() {Name = "Education"},
            new() {Name = "Fashion"},
            new() {Name = "Film"},
            new() {Name = "Media"},
            new() {Name = "Food"},
            new() {Name = "Politics"},
            new() {Name = "Health"},
            new() {Name = "Hobbies"},
            new() {Name = "Lifestyle"},
            new() {Name = "Other"},
            new() {Name = "Performing"},
            new() {Name = "Visual Arts"},
            new() {Name = "Religion"},
            new() {Name = "Science"},
            new() {Name = "Technology"},
            new() {Name = "Seasonal"},
            new() {Name = "Sport"},
            new() {Name = "Outdoor"},
            new() {Name = "Travel"},
            new() {Name = "Automobile"}
        };
    }
}