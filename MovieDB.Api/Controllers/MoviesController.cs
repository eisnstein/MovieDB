using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Helpers;
using MovieDB.Shared.Models.Movies;
using MovieDB.Api.Services;

namespace MovieDB.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MoviesController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMovieService _movieService;

    public MoviesController(IMapper mapper, IMovieService movieService)
    {
        _mapper = mapper;
        _movieService = movieService;
    }

    [HttpGet]
    public ActionResult<List<MovieResponse>> GetAll()
    {
        var movies = _movieService.GetAllAsync(Account!);
        return _mapper.Map<List<MovieResponse>>(movies);
    }

    [HttpGet("{id:int}", Name = "GetMovie")]
    public async Task<ActionResult<MovieResponse>> GetById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id, Account!);
        return _mapper.Map<MovieResponse>(movie);
    }

    [HttpPost]
    public async Task<ActionResult<MovieResponse>> Create(CreateRequest model)
    {
        var movie = await _movieService.CreateAsync(model, Account!);
        var response = _mapper.Map<MovieResponse>(movie);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MovieResponse>> Update(int id, UpdateRequest model)
    {
        var movie = await _movieService.UpdateAsync(id, model, Account!);
        var response = _mapper.Map<MovieResponse>(movie);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _movieService.DeleteByIdAsync(id, Account!);
        return NoContent();
    }
}
