using System;
using OpenEvent.Data.Models.Address;

namespace OpenEvent.Web.Helpers
{
    /// <summary>
    /// Class for calculating distance between the user and events.
    /// </summary>
    public static class CalculateDistance
    {
        /// <summary>
        /// Method for calculating the distance between two coordinates.
        /// </summary>
        /// <param name="point1">Geo coordinate</param>
        /// <param name="point2">Geo coordinate</param>
        /// <returns>
        /// Returns the distance in meters.
        /// </returns>
        public static double Calculate(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}