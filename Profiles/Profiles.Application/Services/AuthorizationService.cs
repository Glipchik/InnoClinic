using Profiles.Application.Exceptions;
using Profiles.Application.Models;
using Profiles.Application.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace Profiles.Application.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _authorizationServerUrl;
        private readonly string _authorizationClientUrl;
        private readonly string _clientSecret;
        private readonly string _clientId;

        public AuthorizationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authorizationServerUrl = configuration["Authorization:ServerUrl"] ?? throw new ArgumentNullException("Authorization:ServerUrl string is null");
            _authorizationClientUrl = configuration["ProfilesAuthM2M:ServerUrl"] ?? throw new ArgumentNullException("ProfilesAuthM2M:ServerUrl string is null");
            _clientSecret = configuration["ProfilesAuthM2M:ClientSecret"] ?? throw new ArgumentNullException("ProfilesAuthM2M:ClientSecret string is null");
            _clientId = configuration["ProfilesAuthM2M:ClientId"] ?? throw new ArgumentNullException("ProfilesAuthM2M:ClientId string is null");
        }

        public async Task<AuthorizationAccountModel> CreateAccount(CreateAccountAuthorizationServerModel createAccountAuthorizationServerModel, CancellationToken cancellationToken)
        {
            var accessToken = await GetAccessTokenAsync(cancellationToken);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_authorizationClientUrl}api/Accounts")
            {
                Content = JsonContent.Create(createAccountAuthorizationServerModel)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException(response.ReasonPhrase);
                }
            }

            var newResponse = await response.Content.ReadAsStringAsync();

            var accountModel = await response.Content.ReadFromJsonAsync<AuthorizationAccountModel>(cancellationToken: cancellationToken);
            return accountModel ?? throw new BadRequestException("Account model is null");
        }

        private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest
            {
                Address = $"{_authorizationServerUrl}connect/token",
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                Scope = "create_account"
            };

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest, cancellationToken: cancellationToken);

            if (tokenResponse.IsError)
            {
                throw new Exception("Failed to retrieve access token");
            }

            return tokenResponse.AccessToken ?? throw new Exception("Failed to retrieve access token");
        }

        private class TokenResponse
        {
            public string? AccessToken { get; set; }
        }
    }
}
