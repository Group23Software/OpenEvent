using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using OpenEvent.Test.Setups;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Promo;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test
{
    public class TestData
    {
        public class TestUserData
        {
            public static Faker<User> FakeUser = new Faker<User>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.UserName, f => f.Person.UserName)
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
                .RuleFor(x => x.IsDarkMode, f => f.Random.Bool())
                .RuleFor(x => x.Avatar, f => f.Random.Bytes(1000))
                .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth);
        }

        public class TestEventData
        {
            public static Faker<Event> FakeEvent = new Faker<Event>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Name, f => f.Random.String())
                .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Address, () => FakeAddress.Generate())
                .RuleFor(x => x.isCanceled, () => false)
                .RuleFor(x => x.IsOnline, () => true)
                .RuleFor(x => x.Images, () => FakeImage.Generate(6).ToList())
                .RuleFor(x => x.Thumbnail, () => FakeImage.Generate())
                .RuleFor(x => x.SocialLinks, () => FakeSocialLink.Generate(1).ToList())
                .RuleFor(x => x.StartLocal, f => f.Date.Soon())
                .RuleFor(x => x.EndLocal, f => f.Date.Future())
                .RuleFor(x => x.Price, f => f.Random.Int(0));
        }

        public static Faker<Image> FakeImage = new Faker<Image>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Label, f => f.Lorem.Slug())
            .RuleFor(x => x.Source, f => f.Random.Bytes(1000));

        public static Faker<ImageViewModel> FakeImageViewModel = new Faker<ImageViewModel>()
            .RuleFor(x => x.Label, f => f.Lorem.Slug())
            .RuleFor(x => x.Source, f => f.Image.LoremPixelUrl());

        public static Faker<Address> FakeAddress = new Faker<Address>()
            .RuleFor(x => x.City, f => f.Address.City())
            .RuleFor(x => x.AddressLine1, f => f.Address.StreetName())
            .RuleFor(x => x.AddressLine2, f => f.Address.BuildingNumber())
            .RuleFor(x => x.CountryCode, f => "GB")
            .RuleFor(x => x.CountryName, f => "United Kingdom")
            .RuleFor(x => x.PostalCode, f => f.Address.ZipCode("??## #??"));

        public static Faker<SocialLink> FakeSocialLink = new Faker<SocialLink>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Link, f => f.Internet.Url())
            .RuleFor(x => x.SocialMedia, f => f.PickRandom<SocialMedia>());

        public static Faker<SocialLinkViewModel> FakeSocialLinkViewModel = new Faker<SocialLinkViewModel>()
            .RuleFor(x => x.Link, f => f.Internet.Url())
            .RuleFor(x => x.SocialMedia, f => f.PickRandom<SocialMedia>());

        public static Faker<Category> FakeCategory = new Faker<Category>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0]);

        public static Faker<CreateEventBody> FakeCreateEventBody = new Faker<CreateEventBody>()
            .RuleFor(x => x.Name, f => f.Company.CompanyName())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Price, f => f.Random.Int(0))
            .RuleFor(x => x.IsOnline, f => f.Random.Bool())
            .RuleFor(x => x.NumberOfTickets, f => f.Random.Int(0, 1000))
            .RuleFor(x => x.StartLocal, f => f.Date.Soon())
            .RuleFor(x => x.EndLocal, f => f.Date.Future())
            .RuleFor(x => x.HostId, () => new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"))
            .RuleFor(x => x.Thumbnail, () => FakeImageViewModel.Generate())
            .RuleFor(x => x.Images, () => FakeImageViewModel.Generate(6))
            .RuleFor(x => x.Address, () => FakeAddress.Generate())
            .RuleFor(x => x.SocialLinks, f => new List<SocialLinkViewModel>() {FakeSocialLinkViewModel.Generate()});

        public static Faker<CreatePromoBody> FakeCreatePromoBody = new Faker<CreatePromoBody>()
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.Discount, f => f.Random.Double(0,1))
            .RuleFor(x => x.Start, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(10)))
            .RuleFor(x => x.End, f => f.Date.Between(DateTime.Now.AddDays(15),DateTime.Now.AddMonths(4)));
    }
}