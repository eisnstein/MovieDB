using System.Net;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Tests.Factories;

namespace MovieDB.Tests.Integration;

public class AccountsEndpointTests
{
    private static TestApplication? _application;
    private static HttpClient? _client;

    [Before(Class)]
    public static void Setup()
    {
        _application = new TestApplication();
        _client = _application.CreateClient();
    }

    [After(Class)]
    public static void Teardown()
    {
        _application?.Dispose();
    }

    [Test]
    public async Task user_cannot_register_with_invalid_email()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/register", new
        {
            email = "daniel",
            password = "password",
            confirmPassword = "password"
        });

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        await Assert.That(validationResult).IsNotNull();
        await Assert.That(validationResult!.Errors["Email"][0]).IsEqualTo("The Email field is not a valid e-mail address.");
    }

    [Test]
    public async Task user_cannot_register_with_invalid_password()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/register", new RegisterRequest
        {
            Email = "daniel@test.local",
            Password = "short",
            ConfirmPassword = "short"
        });

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        await Assert.That(validationResult).IsNotNull();
        await Assert.That(validationResult!.Errors["Password"][0]).IsEqualTo("The field Password must be a string or array type with a minimum length of '8'.");
    }

    [Test]
    public async Task user_cannot_register_with_not_equal_passwords()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/register", new RegisterRequest
        {
            Email = "daniel@test.local",
            Password = "password",
            ConfirmPassword = "confirm-password"
        });

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        await Assert.That(validationResult).IsNotNull();
        await Assert.That(validationResult!.Errors["ConfirmPassword"][0]).IsEqualTo("'ConfirmPassword' and 'Password' do not match.");
    }

    [Test]
    public async Task user_cannot_authenticate_without_password()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
        {
            Email = "daniel@test.local"
        });

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        await Assert.That(validationResult).IsNotNull();
        await Assert.That(validationResult!.Errors["Password"][0]).IsEqualTo("The Password field is required.");
    }

    [Test]
    public async Task user_cannot_authenticate_with_invalid_email_address()
    {
        var result = await _client.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
        {
            Email = "daniel",
            Password = "password"
        });

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        await Assert.That(validationResult).IsNotNull();
        await Assert.That(validationResult!.Errors["Email"][0]).IsEqualTo("The Email field is not a valid e-mail address.");
    }

    [Test]
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

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var responseBody = await result.Content.ReadAsStringAsync();
        await Assert.That(responseBody).IsNotNull();
        await Assert.That(responseBody).IsEqualTo(@"{""message"":""Email or password wrong""}");
    }
}
