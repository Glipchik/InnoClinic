using Authorization.Application.Exceptions;
using Authorization.Application.Models;
using Authorization.Application.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace Authorization.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _authorizationServerUrl;
        private readonly string _profileServerUrl;
        private readonly string _clientSecret;
        private readonly string _clientId;

        public ProfileService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authorizationServerUrl = configuration["AuthorizationServerUrl"] ?? throw new ArgumentNullException("AuthorizationServerUrl string is null");
            _profileServerUrl = configuration["AuthorizationClients:AuthProfilesM2M:ProfilesApiBaseUrl"] ?? throw new ArgumentNullException("AuthorizationClients:AuthProfilesM2M:ProfilesApiBaseUrl string is null");
            _clientSecret = configuration["AuthorizationClientSecrets:AuthProfilesM2M:ClientSecret"] ?? throw new ArgumentNullException("AuthorizationClientSecrets:AuthProfilesM2M:ClientSecret string is null");
            _clientId = configuration["AuthorizationClients:AuthProfilesM2M:ClientId"] ?? throw new ArgumentNullException("AuthorizationClients:AuthProfilesM2M:ClientId string is null");
        }

        private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest
            {
                Address = $"{_authorizationServerUrl}connect/token",
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                Scope = "create_patient_profile"
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
            public string AccessToken { get; set; }
        }

        public async Task CreatePatientProfile(CreatePatientModel createPatientModel, CreateAccountForProfilesApiModel createAccountForProfilesApiModel, CancellationToken cancellationToken)
        {
            var accessToken = await GetAccessTokenAsync(cancellationToken);

            using var content = new MultipartFormDataContent
            {
                { new StringContent(createAccountForProfilesApiModel.Id.ToString()), "CreateAccountFromAuthDto.Id" },
                { new StringContent(createAccountForProfilesApiModel.Email), "CreateAccountFromAuthDto.Email" },
                { new StringContent(createAccountForProfilesApiModel.PhoneNumber), "CreateAccountFromAuthDto.PhoneNumber" },
                { new StringContent(createPatientModel.FirstName), "FirstName" },
                { new StringContent(createPatientModel.LastName), "LastName" },
                { new StringContent(createPatientModel.MiddleName ?? string.Empty), "MiddleName" },
                { new StringContent(createPatientModel.DateOfBirth.ToString("o")), "DateOfBirth" }
            };

            if (createPatientModel.ProfilePicture != null)
            {
                var fileContent = new StreamContent(createPatientModel.ProfilePicture.FileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(createPatientModel.ProfilePicture.ContentType);
                content.Add(fileContent, "photo", createPatientModel.ProfilePicture.FileName);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_profileServerUrl}api/Authorization")
            {
                Content = content
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new BadRequestException($"Request failed: {response.StatusCode} - {errorContent}");
                }
            }
        }
    }
}
