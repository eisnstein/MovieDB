using AutoMapper;
using MovieDB.Api.App.Entities;
using MovieDB.Api.App.Services;
using MovieDB.Shared.Models.Theaters;
using TheaterCreateRequest = MovieDB.Shared.Models.Theaters.CreateRequest;
using TheaterUpdateRequest = MovieDB.Shared.Models.Theaters.UpdateRequest;

namespace MovieDB.Api.App.Endpoints;

public static class TheatersEndpoints
{
    public static IResult GetAll(
        HttpContext context,
        IMapper mapper,
        ITheaterService theaterService)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var theaters = theaterService.GetAllAsync(account);
        var mapped = mapper.Map<List<TheaterResponse>>(theaters);

        return Results.Ok(mapped);
    }

    public static async Task<IResult> GetById(
        HttpContext context,
        IMapper mapper,
        ITheaterService theaterService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var theater = await theaterService.GetByIdAsync(id, account);

        return Results.Ok(mapper.Map<TheaterResponse>(theater));
    }

    public static async Task<IResult> Create(
        HttpContext context,
        IMapper mapper,
        ITheaterService theaterService,
        TheaterCreateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var theater = await theaterService.CreateAsync(model, account);
        var response = mapper.Map<TheaterResponse>(theater);

        return Results.CreatedAtRoute(nameof(GetById), new { id = theater.Id }, response);
    }

    public static async Task<IResult> Update(
        HttpContext context,
        IMapper mapper,
        ITheaterService theaterService,
        int id,
        TheaterUpdateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var theater = await theaterService.UpdateAsync(id, model, account);
        var response = mapper.Map<TheaterResponse>(theater);

        return Results.CreatedAtRoute(nameof(GetById), new { id = theater.Id }, response);
    }

    public static async Task<IResult> Delete(
        HttpContext context,
        ITheaterService theaterService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        await theaterService.DeleteByIdAsync(id, account);

        return Results.NoContent();
    }
}
