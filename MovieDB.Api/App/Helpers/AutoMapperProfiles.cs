using AutoMapper;
using MovieDB.Api.App.Entities;
using MovieDB.Shared.Models.Accounts;
using MovieDB.Shared.Models.Concerts;
using MovieDB.Shared.Models.Movies;
using MovieDB.Shared.Models.Theaters;
using UpdateRequestAccount = MovieDB.Shared.Models.Accounts.UpdateRequest;
using UpdateRequestMovie = MovieDB.Shared.Models.Movies.UpdateRequest;
using UpdateRequestConcert = MovieDB.Shared.Models.Concerts.UpdateRequest;
using UpdateRequestTheater = MovieDB.Shared.Models.Theaters.UpdateRequest;
using CreateRequestMovie = MovieDB.Shared.Models.Movies.CreateRequest;
using CreateRequestConcert = MovieDB.Shared.Models.Concerts.CreateRequest;
using CreateRequestTheater = MovieDB.Shared.Models.Theaters.CreateRequest;

namespace MovieDB.Api.App.Helpers;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Account, AccountResponse>();
        CreateMap<Account, AuthenticateResponse>();
        CreateMap<RegisterRequest, Account>();
        CreateMap<UpdateRequestAccount, Account>()
            .ForAllMembers(x => x.Condition(
                (src, _, prop) =>
                {
                    if (prop is null) return false;
                    if (prop is string p && string.IsNullOrEmpty(p)) return false;

                    if (x.DestinationMember.Name == "Role" && src.Role is null) return false;

                    return true;
                }
            ));
    }
}

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<CreateRequestMovie, Movie>();
        CreateMap<Movie, MovieResponse>();
        CreateMap<UpdateRequestMovie, Movie>()
            .ForAllMembers(x => x.Condition((_, _, value) => value switch
            {
                null => false,
                string v when string.IsNullOrEmpty(v) => false,
                _ => true
            }));
    }
}

public class ConcertProfile : Profile
{
    public ConcertProfile()
    {
        CreateMap<CreateRequestConcert, Concert>();
        CreateMap<Concert, ConcertResponse>();
        CreateMap<UpdateRequestMovie, Concert>()
            .ForAllMembers(x => x.Condition((_, _, value) => value switch
            {
                null => false,
                string v when string.IsNullOrEmpty(v) => false,
                _ => true
            }));
    }
}

public class TheaterProfile : Profile
{
    public TheaterProfile()
    {
        CreateMap<CreateRequestTheater, Theater>();
        CreateMap<Theater, TheaterResponse>();
        CreateMap<UpdateRequestTheater, Theater>()
            .ForAllMembers(x => x.Condition((_, _, value) => value switch
            {
                null => false,
                string v when string.IsNullOrEmpty(v) => false,
                _ => true
            }));
    }
}
