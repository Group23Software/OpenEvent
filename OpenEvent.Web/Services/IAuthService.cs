using System;
using System.Threading.Tasks;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service providing all logic for user authentication
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Main login method.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="remember"></param>
        /// <returns>
        /// A task of type <see cref="UserViewModel"/> representing basic user information.
        /// </returns>
        Task<UserViewModel> Login(string email, string password, bool remember);

        /// <summary>
        /// Method for authenticating the user once a token has been obtained.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// A task of type <see cref="UserViewModel"/> representing basic user information.
        /// </returns>
        Task<UserViewModel> Authenticate(Guid id);

        /// <summary>
        /// Method for sending a forgot password email to the user
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>Completed task once the email has been sent</returns>
        Task ForgotPassword(string email);

        /// <summary>
        /// Method for updating updating the user's password.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns>
        /// A completed task once updated.
        /// </returns>
        Task UpdatePassword(Guid id, string password);

        /// <summary>
        /// Method for confirming a user's email
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>Completed task once the user's email has been confirmed</returns>
        Task ConfirmEmail(Guid id);
    }
}