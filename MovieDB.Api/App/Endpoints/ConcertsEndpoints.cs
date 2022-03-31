using AutoMapper;
using MovieDB.Api.App.Entities;
using MovieDB.Api.App.Services;
using MovieDB.Shared.Models.Concerts;
using ConcertCreateRequest = MovieDB.Shared.Models.Concerts.CreateRequest;
using ConcertUpdateRequest = MovieDB.Shared.Models.Concerts.UpdateRequest;

namespace MovieDB.Api.App.Endpoints;

public static class ConcertsEndpoints
{
    public static IResult GetAll(
        HttpContext context,
        IMapper mapper,
        IConcertService concertService)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var concerts = concertService.GetAllAsync(account);
        var mapped = mapper.Map<List<ConcertResponse>>(concerts);

        return Results.Ok(mapped);
    }

    public static async Task<IResult> GetById(
        HttpContext context,
        IMapper mapper,
        IConcertService concertService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var concert = await concertService.GetByIdAsync(id, account);

        return Results.Ok(mapper.Map<ConcertResponse>(concert));
    }

    public static async Task<IResult> Create(
        HttpContext context,
        IMapper mapper,
        IConcertService concertService,
        ConcertCreateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var concert = await concertService.CreateAsync(model, account);
        var response = mapper.Map<ConcertResponse>(concert);

        return Results.CreatedAtRoute(nameof(GetById), new { id = concert.Id }, response);
    }

    public static async Task<IResult> Update(
        HttpContext context,
        IMapper mapper,
        IConcertService concertService,
        int id,
        ConcertUpdateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var concert = await concertService.UpdateAsync(id, model, account);
        var response = mapper.Map<ConcertResponse>(concert);

        return Results.CreatedAtRoute(nameof(GetById), new { id = concert.Id }, response);
    }

    public static async Task<IResult> Delete(
        HttpContext context,
        IConcertService concertService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        await concertService.DeleteByIdAsync(id, account);

        return Results.NoContent();
    }
}
