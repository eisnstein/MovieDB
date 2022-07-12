using System.Net;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Tests.Factories;

namespace MovieDB.Tests.Integration;

public class AccountsEndpointTests
{
    private readonly TestApplication _application;
    private readonly HttpClient _client;

    public AccountsEndpointTests()
    {
        _application = new TestApplication();
        _client = _application.CreateClient();
    }

    [Fact]
    public async Task user_cannot_register_with_invalid_email()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/register", new RegisterRequest
        {
            Email = "daniel",
            Password = "password",
            ConfirmPassword = "password"
        });

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(validationResult);
        Assert.Equal("The Email field is not a valid e-mail address.", validationResult.Errors["Email"][0]);
    }

    [Fact]
    public async Task user_cannot_register_with_invalid_password()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/register", new RegisterRequest
        {
            Email = "daniel@test.local",
            Password = "short",
            ConfirmPassword = "short"
        });

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(validationResult);
        Assert.Equal("The field Password must be a string or array type with a minimum length of '8'.", validationResult.Errors["Password"][0]);
    }

    [Fact]
    public async Task user_cannot_register_with_not_equal_passwords()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/register", new RegisterRequest
        {
            Email = "daniel@test.local",
            Password = "password",
            ConfirmPassword = "confirm-password"
        });

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(validationResult);
        Assert.Equal("'ConfirmPassword' and 'Password' do not match.", validationResult.Errors["ConfirmPassword"][0]);
    }

    [Fact]
    public async Task user_cannot_authenticate_without_password()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
        {
            Email = "daniel@test.local"
        });

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(validationResult);
        Assert.Equal("The Password field is required.", validationResult.Errors["Password"][0]);
    }

    [Fact]
    public async Task user_cannot_authenticate_with_invalid_email_address()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
        {
            Email = "daniel",
            Password = "password"
        });

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(validationResult);
        Assert.Equal("The Email field is not a valid e-mail address.", validationResult.Errors["Email"][0]);
    }

    [Fact]
    public async Task user_cannot_authenticate_with_invalid_password()
    {
        await using var db = _application.Services.GetService<AppDbContext>();
        var account = AccountFactory.CreateAccount();
        db!.Accounts.Add(account);
        await db.SaveChangesAsync();

        var result = await _client.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
        {
            Email = "john@test.local",
            Password = "wrong-password"
        });

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var responseBody = await result.Content.ReadAsStringAsync();
        Assert.NotNull(responseBody);
        Assert.Equal(@"{""message"":""Email or password wrong""}", responseBody);
    }
}
