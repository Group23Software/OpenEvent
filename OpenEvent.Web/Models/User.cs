using System;
using Microsoft.AspNetCore.Identity;

namespace OpenEvent.Web.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        // TODO: Email confirm
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class NewUserInput
    {
        public string Email { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public bool Remember { get; set; }
    }

    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }

    public class UserAccountModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}