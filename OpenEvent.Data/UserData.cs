using System.Text;
using Bogus;
using OpenEvent.Data.Models.User;

namespace OpenEvent.Data
{
    public static class UserData
    {
        public static Faker<User> FakeUser = new Faker<User>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.UserName, f => f.Person.UserName)
            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.IsDarkMode, f => f.Random.Bool())
            .RuleFor(x => x.Avatar, f => Encoding.UTF8.GetBytes(f.Image.PicsumUrl()))
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth)
            .RuleFor(x => x.Confirmed, f => true)
            .RuleFor(x => x.Address, f => Data.FakeAddress.Generate());

        public static Faker<NewUserBody> FakeNewUser = new Faker<NewUserBody>()
            .RuleFor(x => x.UserName, f => f.Person.UserName)
            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth)
            .RuleFor(x => x.Password, f => "Password");
    }
}