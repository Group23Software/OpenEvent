using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OpenEvent.Data.Models.Address;

namespace OpenEvent.Test
{
    [TestFixture]
    public class CalculateDistance
    {
        [Test]
        public async Task Should_Calculate_Distance() {
        {
            var result = Web.Helpers.CalculateDistance.Calculate(new Location()
            {
                Latitude = 51.47338,
                Longitude = -0.08375
            },new Location()
            {
                Latitude = 51.47353,
                Longitude = -0.08069
            });
            result.Should().BeGreaterOrEqualTo(212);
        }}
    }
}