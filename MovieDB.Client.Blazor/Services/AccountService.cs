using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MovieDB.Shared.Models.Accounts;

namespace MovieDB.Client.Blazor.Services
{
    public interface IAccountService
    {
        AuthenticateResponse? User { get; }
        bool IsAuthenticated();
        Task Initialize();
        Task Authenticate(AuthenticateRequest model);
        Task Logout();
        Task<AccountResponse> GetById(int id);
    }

    public class AccountService : IAccountService
    {
        private readonly HttpClient _client;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;
        private const string _userKey = "user";
        private const string _jwtKey = "jwt";

        public AuthenticateResponse? User { get; private set; }

        public AccountService(
            HttpClient client,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService)
        {
            _client = client;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public bool IsAuthenticated()
        {
            return User is not null;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<AuthenticateResponse>(_userKey);
        }

        public async Task Authenticate(AuthenticateRequest model)
        {
            var response = await _client.PostAsJsonAsync("/api/accounts/authenticate", model);
            User = await response.Content.ReadFromJsonAsync<AuthenticateResponse>();
            await _localStorageService.SetItem(_userKey, User);
            await _localStorageService.SetItem(_jwtKey, User.JwtToken);
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem(_userKey);
        }

        public async Task<AccountResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
