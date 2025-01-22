using Authorization.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Authorization.Application.Services.Abstractions
{
    public interface IAccountService
    {
        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="credentialsModel">The credentials model containing the accountname and password.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns <c>true</c> if the credentials are valid; otherwise, <c>false</c>.</returns>
        Task<bool> AreCredentialsValid(CredentialsModel credentialsModel, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the account by identifier.
        /// </summary>
        /// <param name="guid">The account identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<AccountModel> FindById(Guid guid, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the account by email.
        /// </summary>
        /// <param name="email">The account email.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<AccountModel> FindByEmail(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new a accunt.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="createAccountModel">User information for creation.</param>
        /// <returns></returns>
        Task<AccountModel> CreateAccount(CreateAccountModel createAccountModel, CancellationToken cancellationToken,
            CreatePatientModel? createPatientModel = null,
            bool isCreatingPatientRequired = true);
    }
}