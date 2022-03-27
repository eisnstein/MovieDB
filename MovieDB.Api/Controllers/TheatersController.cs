using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Helpers;
using MovieDB.Api.Services;
using MovieDB.Shared.Models.Theaters;

namespace MovieDB.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheatersController : BaseController
{
    private readonly IMapper _mapper;
    private readonly ITheaterService _theaterService;

    public TheatersController(IMapper mapper, ITheaterService theaterService)
    {
        _mapper = mapper;
        _theaterService = theaterService;
    }

    [HttpGet]
    public ActionResult<List<TheaterResponse>> GetAll()
    {
        var theaters = _theaterService.GetAllAsync(Account!);
        return _mapper.Map<List<TheaterResponse>>(theaters);
    }

    [HttpGet("{id:int}", Name = "GetTheater")]
    public async Task<ActionResult<TheaterResponse>> GetById(int id)
    {
        var theater = await _theaterService.GetByIdAsync(id, Account!);
        return _mapper.Map<TheaterResponse>(theater);
    }

    [HttpPost]
    public async Task<ActionResult<TheaterResponse>> Create(CreateRequest model)
    {
        var theater = await _theaterService.CreateAsync(model, Account!);
        var response = _mapper.Map<TheaterResponse>(theater);
        return CreatedAtAction(nameof(GetById), new { id = theater.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TheaterResponse>> Update(int id, UpdateRequest model)
    {
        var theater = await _theaterService.UpdateAsync(id, model, Account!);
        var response = _mapper.Map<TheaterResponse>(theater);
        return CreatedAtAction(nameof(GetById), new { id = theater.Id }, response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _theaterService.DeleteByIdAsync(id, Account!);
        return NoContent();
    }
}
