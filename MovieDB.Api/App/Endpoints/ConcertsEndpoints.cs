using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieDB.Api.App.Entities;
using MovieDB.Api.App.Services;
using MovieDB.Shared.Models.Concerts;
using ConcertCreateRequest = MovieDB.Shared.Models.Concerts.CreateRequest;
using ConcertUpdateRequest = MovieDB.Shared.Models.Concerts.UpdateRequest;

namespace MovieDB.Api.App.Endpoints;

public static class ConcertsEndpoints
{
    public static Ok<List<ConcertResponse>> GetAll(
        HttpContext context,
        IMapper mapper,
        IConcertService concertService)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var concerts = concertService.GetAllAsync(account);
        var mapped = mapper.Map<List<ConcertResponse>>(concerts);

        return TypedResults.Ok(mapped);
    }

    public static async Task<Ok<ConcertResponse>> GetById(
        HttpContext context,
        IMapper mapper,
        IConcertService concertService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var concert = await concertService.GetByIdAsync(id, account);
        var response = mapper.Map<ConcertResponse>(concert);

        return TypedResults.Ok(response);
    }

    public static async Task<CreatedAtRoute<ConcertResponse>> Create(
        HttpContext context,
        IMapper mapper,
        IConcertService concertService,
        ConcertCreateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var concert = await concertService.CreateAsync(model, account);
        var response = mapper.Map<ConcertResponse>(concert);

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<CreatedAtRoute<ConcertResponse>> Update(
        HttpContext context,
        IMapper mapper,
        IConcertService concertService,
        int id,
        ConcertUpdateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var concert = await concertService.UpdateAsync(id, model, account);
        var response = mapper.Map<ConcertResponse>(concert);

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<NoContent> Delete(
        HttpContext context,
        IConcertService concertService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        await concertService.DeleteByIdAsync(id, account);

        return TypedResults.NoContent();
    }
}
