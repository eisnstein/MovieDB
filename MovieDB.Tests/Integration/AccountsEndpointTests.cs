using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Models;
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
        var result = await _client!.PostAsJsonAsync("/api/accounts/register", new
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
        var result = await _client!.PostAsJsonAsync("/api/accounts/register", new RegisterRequest
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
        var result = await _client!.PostAsJsonAsync("/api/accounts/register", new RegisterRequest
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
        var result = await _client!.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
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
        var result = await _client!.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
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
        await AddAccountAsync(AccountFactory.CreateAccount());

        var result = await _client!.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
        {
            Email = "john@test.local",
            Password = "wrong-password"
        });

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var responseBody = await result.Content.ReadAsStringAsync();
        await Assert.That(responseBody).IsNotNull();
        await Assert.That(responseBody).IsEqualTo(@"{""message"":""Email or password wrong""}");
    }

    [Test]
    public async Task authenticate_sets_http_only_refresh_token_cookie()
    {
        var client = await CreateClientWithAccountAsync(2, "refresh-cookie@test.local");

        var result = await AuthenticateAsync(client, "refresh-cookie@test.local");

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var setCookie = GetRefreshTokenSetCookie(result)!.ToLowerInvariant();
        await Assert.That(setCookie).Contains("httponly");
        await Assert.That(setCookie).Contains("secure");
        await Assert.That(setCookie).Contains("samesite=strict");
        await Assert.That(setCookie).Contains("path=/api/accounts");
    }

    [Test]
    public async Task refresh_token_rotates_and_the_new_token_can_be_used_again()
    {
        var client = await CreateClientWithAccountAsync(3, "refresh-rotate@test.local");
        var firstToken = GetRefreshToken(await AuthenticateAsync(client, "refresh-rotate@test.local"));

        var firstRefresh = await RefreshAsync(client, firstToken);
        await Assert.That(firstRefresh.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var body = await firstRefresh.Content.ReadFromJsonAsync<JsonElement>();
        await Assert.That(body.GetProperty("jwtToken").GetString()).IsNotNullOrEmpty();
        await Assert.That(body.TryGetProperty("refreshToken", out _)).IsFalse();

        var secondToken = GetRefreshToken(firstRefresh);
        await Assert.That(secondToken).IsNotEqualTo(firstToken);

        var secondRefresh = await RefreshAsync(client, secondToken);
        await Assert.That(secondRefresh.StatusCode).IsEqualTo(HttpStatusCode.OK);

        // The first token was replaced, so it can't be used anymore
        var reusedRefresh = await RefreshAsync(client, firstToken);
        await Assert.That(reusedRefresh.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task refresh_token_is_required()
    {
        var client = _application!.CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = false });

        var result = await client.PostAsync("/api/accounts/refresh-token", null);

        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task revoked_refresh_token_cannot_be_used()
    {
        var client = await CreateClientWithAccountAsync(4, "refresh-revoke@test.local");
        var login = await AuthenticateAsync(client, "refresh-revoke@test.local");
        var refreshToken = GetRefreshToken(login);
        var jwtToken = (await login.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("jwtToken").GetString();

        var revoke = new HttpRequestMessage(HttpMethod.Post, "/api/accounts/revoke-token")
        {
            Content = JsonContent.Create(new { })
        };
        revoke.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        revoke.Headers.Add("Cookie", $"refreshToken={refreshToken}");
        var revokeResult = await client.SendAsync(revoke);

        await Assert.That(revokeResult.StatusCode).IsEqualTo(HttpStatusCode.OK);
        // The cookie is cleared, too
        await Assert.That(GetRefreshToken(revokeResult)).IsEqualTo(string.Empty);

        var refresh = await RefreshAsync(client, refreshToken);
        await Assert.That(refresh.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    // Helpers

    // Cookies are handled by hand: the cookie is Secure, and the test server runs on plain http
    private static async Task<HttpClient> CreateClientWithAccountAsync(int id, string email)
    {
        await AddAccountAsync(AccountFactory.CreateAccount(id, email, verified: true));

        return _application!.CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = false });
    }

    // Tests run in parallel, so each one gets its own scoped DbContext
    private static async Task AddAccountAsync(Account account)
    {
        using var scope = _application!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Accounts.Add(account);
        await db.SaveChangesAsync();
    }

    private static Task<HttpResponseMessage> AuthenticateAsync(HttpClient client, string email)
        => client.PostAsJsonAsync("/api/accounts/authenticate", new AuthenticateRequest()
        {
            Email = email,
            Password = "password"
        });

    private static Task<HttpResponseMessage> RefreshAsync(HttpClient client, string refreshToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/accounts/refresh-token");
        request.Headers.Add("Cookie", $"refreshToken={refreshToken}");
        return client.SendAsync(request);
    }

    private static string? GetRefreshTokenSetCookie(HttpResponseMessage response)
        => response.Headers.TryGetValues("Set-Cookie", out var values)
            ? values.FirstOrDefault(v => v.StartsWith("refreshToken="))
            : null;

    private static string GetRefreshToken(HttpResponseMessage response)
    {
        var setCookie = GetRefreshTokenSetCookie(response) ?? throw new InvalidOperationException("No refreshToken cookie set");
        return setCookie.Split(';')[0]["refreshToken=".Length..];
    }
}
