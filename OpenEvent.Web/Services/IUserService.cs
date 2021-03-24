using System;
using System.Threading.Tasks;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// UserService interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Method for creating a new user.
        /// </summary>
        /// <param name="userBody">All user data collected for signup <see cref="NewUserBody"/>.</param>
        /// <returns>
        /// A completed task once saved and Email has been sent
        /// </returns>
        Task Create(NewUserBody userBody);

        /// <summary>
        /// Method for removing user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// A completed Task once deleted.
        /// </returns>
        Task Destroy(Guid id);

        /// <summary>
        /// Method for getting user data for the account page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Task of type <see cref="UserAccountModel"/> representing all data needed for account page.
        /// </returns>
        Task<UserAccountModel> Get(Guid id);

        /// <summary>
        /// Gets all the users analytics
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>
        /// Users analytics including: page views, searches, recommendation scores and ticket verifications 
        /// </returns>
        Task<UsersAnalytics> GetAnalytics(Guid id);

        /// <summary>
        /// Method for updating the users avatar.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="avatar">byte array of bitmap image</param>
        /// <returns>
        /// Task of string encoded bitmap.
        /// </returns>
        Task<string> UpdateAvatar(Guid id, byte[] avatar);

        /// <summary>
        /// Method for updating the users username.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>
        /// Task of username string.
        /// </returns>
        Task<string> UpdateUserName(Guid id, string name);

        /// <summary>
        /// Method for checking if a user with a username exists.
        /// </summary>
        /// <param name="username">username to check</param>
        /// <returns>
        /// Task of bool if username exists.
        /// </returns>
        Task<bool> UserNameExists(string username);

        /// <summary>
        /// Method for checking if a user with a email exists.
        /// </summary>
        /// <param name="email">email to check</param>
        /// <returns>
        /// Task of bool if email exists.
        /// </returns>
        Task<bool> EmailExists(string email);

        /// <summary>
        /// Method for checking if a user with a phone number exists.
        /// </summary>
        /// <param name="phoneNumber">phone number to check</param>
        /// <returns>
        /// Task of bool if phone number exists.
        /// </returns>
        Task<bool> PhoneExists(string phoneNumber);

        /// <summary>
        /// Updates users theme preference.
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="isDarkMode">If the user prefers dark mode</param>
        /// <returns></returns>
        Task<bool> UpdateThemePreference(Guid id, bool isDarkMode);

        /// <summary>
        /// Determines if the user owns the event supplied.
        /// </summary>
        /// <param name="eventId">Event's id</param>
        /// <param name="userId">User's id</param>
        /// <returns>
        /// bool if the user is the host of the event
        /// </returns>
        Task<bool> HostOwnsEvent(Guid eventId, Guid userId);

        /// <summary>
        /// Updates users address.
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="address">User's new address</param>
        /// <returns>
        /// Returns the updated address
        /// </returns>
        Task<Address> UpdateAddress(Guid id, Address address);
    }
}