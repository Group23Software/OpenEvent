using System;
using System.Linq;
using Bogus;
using OpenEvent.Data.Models.Event;

namespace OpenEvent.Data
{
    public static class EventData
    {
        public static Faker<Event> FakeEvent = new Faker<Event>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Name, f => f.Lorem.Sentence())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Address, () => Data.FakeAddress.Generate())
            .RuleFor(x => x.isCanceled, () => false)
            .RuleFor(x => x.IsOnline, () => false)
            .RuleFor(x => x.Images, () => Data.FakeImage.Generate(6).ToList())
            .RuleFor(x => x.Thumbnail, () => Data.FakeImage.Generate())
            .RuleFor(x => x.SocialLinks, () => Data.FakeSocialLink.Generate(1).ToList())
            .RuleFor(x => x.StartLocal, f => f.Date.Soon())
            .RuleFor(x => x.EndLocal, f => f.Date.Future())
            .RuleFor(x => x.Created, f => f.Date.Between(DateTime.Now.AddMonths(-3), DateTime.Now))
            .RuleFor(x => x.Price, f => f.Random.Long(0,100000));

        public static Faker<CreateEventBody> FakeCreateEvent = new Faker<CreateEventBody>()
            .RuleFor(x => x.Name, f => f.Lorem.Sentence())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
            // .RuleFor(x => x.Address, () => Data.FakeAddress.Generate())
            .RuleFor(x => x.NumberOfTickets, f => f.Random.Int(100))
            .RuleFor(x => x.Price, f => f.Random.Long(1000000))
            .RuleFor(x => x.IsOnline, () => true)
            .RuleFor(x => x.StartLocal, f => f.Date.Soon())
            .RuleFor(x => x.EndLocal, f => f.Date.Future())
            .RuleFor(x => x.Images, () => Data.FakeImageViewModel.Generate(6).ToList())
            .RuleFor(x => x.Thumbnail, () => Data.FakeImageViewModel.Generate())
            .RuleFor(x => x.SocialLinks, () => Data.FakeSocialLinkViewModel.Generate(1).ToList());
    }
}