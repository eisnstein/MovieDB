using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Http.Responses;
using MovieDB.Api.App.Models;
using MovieDB.Api.App.Services;

namespace MovieDB.Api.App.Endpoints;

public static class TheatersEndpoints
{
    public static Ok<List<TheaterResponse>> GetAll(
        HttpContext context,
        IMapper mapper,
        ITheaterService theaterService)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var theaters = theaterService.GetAllAsync(account);
        var mapped = mapper.Map<List<TheaterResponse>>(theaters);

        return TypedResults.Ok(mapped);
    }

    public static async Task<Ok<TheaterResponse>> GetById(
        HttpContext context,
        IMapper mapper,
        ITheaterService theaterService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var theater = await theaterService.GetByIdAsync(id, account);
        var response = mapper.Map<TheaterResponse>(theater);

        return TypedResults.Ok(response);
    }

    public static async Task<CreatedAtRoute<TheaterResponse>> Create(
        HttpContext context,
        IMapper mapper,
        ITheaterService theaterService,
        TheaterCreateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var theater = await theaterService.CreateAsync(model, account);
        var response = mapper.Map<TheaterResponse>(theater);

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<CreatedAtRoute<TheaterResponse>> Update(
        HttpContext context,
        IMapper mapper,
        ITheaterService theaterService,
        int id,
        TheaterUpdateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var theater = await theaterService.UpdateAsync(id, model, account);
        var response = mapper.Map<TheaterResponse>(theater);

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<NoContent> Delete(
        HttpContext context,
        ITheaterService theaterService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        await theaterService.DeleteByIdAsync(id, account);

        return TypedResults.NoContent();
    }
}
