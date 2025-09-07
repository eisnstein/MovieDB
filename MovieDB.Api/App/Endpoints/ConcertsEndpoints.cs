using Microsoft.AspNetCore.Http.HttpResults;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Http.Responses;
using MovieDB.Api.App.Models;
using MovieDB.Api.App.Services;

namespace MovieDB.Api.App.Endpoints;

public static class ConcertsEndpoints
{
    public static Ok<List<ConcertResponse>> GetAll(
        HttpContext context,
        IConcertService concertService)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        var concerts = concertService.GetAllAsync(account);

        var response = concerts.Select(c => new ConcertResponse
        {
            Id = c.Id,
            Title = c.Title,
            SeenAt = c.SeenAt,
            Location = c.Location,
            Genre = (int)c.Genre,
            Rating = (int)c.Rating,
        }).ToList();

        return TypedResults.Ok(response);
    }

    public static async Task<Ok<ConcertResponse>> GetById(
        HttpContext context,
        IConcertService concertService,
        int id)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        var concert = await concertService.GetByIdAsync(id, account);

        var response = new ConcertResponse
        {
            Id = concert.Id,
            Title = concert.Title,
            SeenAt = concert.SeenAt,
            Location = concert.Location,
            Genre = (int)concert.Genre,
            Rating = (int)concert.Rating,
        };

        return TypedResults.Ok(response);
    }

    public static async Task<CreatedAtRoute<ConcertResponse>> Create(
        HttpContext context,
        IConcertService concertService,
        ConcertCreateRequest model)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        var concert = await concertService.CreateAsync(model, account);

        var response = new ConcertResponse
        {
            Id = concert.Id,
            Title = concert.Title,
            SeenAt = concert.SeenAt,
            Location = concert.Location,
            Genre = (int)concert.Genre,
            Rating = (int)concert.Rating,
        };

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<CreatedAtRoute<ConcertResponse>> Update(
        HttpContext context,
        IConcertService concertService,
        int id,
        ConcertUpdateRequest model)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        var concert = await concertService.UpdateAsync(id, model, account);

        var response = new ConcertResponse
        {
            Id = concert.Id,
            Title = concert.Title,
            SeenAt = concert.SeenAt,
            Location = concert.Location,
            Genre = (int)concert.Genre,
            Rating = (int)concert.Rating,
        };

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<NoContent> Delete(
        HttpContext context,
        IConcertService concertService,
        int id)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        await concertService.DeleteByIdAsync(id, account);

        return TypedResults.NoContent();
    }
}
