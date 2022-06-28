using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MovieDB.Shared.Models.Accounts;

namespace MovieDB.Tests;

public class AccountsEndpointTest
{
    [Fact]
    public async Task user_cannot_authenticate_without_password()
    {
        await using var application = new TestApplication();
        using var client = application.CreateClient();

        var result = await client.PostAsJsonAsync("/api/accounts/authenticate", new RegisterRequest
        {
            Email = "daniel@test.local"
        });

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(validationResult);
        Assert.Equal("The Password field is required.", validationResult.Errors["Password"][0]);
    }

    [Fact]
    public async Task user_cannot_authenticate_with_invalid_email_adress()
    {
        await using var application = new TestApplication();
        using var client = application.CreateClient();

        var result = await client.PostAsJsonAsync("/api/accounts/authenticate", new RegisterRequest
        {
            Email = "daniel",
            Password = "password"
        });

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        var validationResult = await result.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(validationResult);
        Assert.Equal("The Email field is not a valid e-mail address.", validationResult.Errors["Email"][0]);
    }
}
