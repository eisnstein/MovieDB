using AutoMapper;
using MovieDB.Api.Entities;
using MovieDB.Api.Models.Accounts;
using MovieDB.Api.Models.Concerts;
using MovieDB.Api.Models.Movies;
using MovieDB.Api.Models.Theaters;
using UpdateRequestAccount = MovieDB.Api.Models.Accounts.UpdateRequest;
using UpdateRequestMovie = MovieDB.Api.Models.Movies.UpdateRequest;
using UpdateRequestConcert = MovieDB.Api.Models.Concerts.UpdateRequest;
using UpdateRequestTheater = MovieDB.Api.Models.Theaters.UpdateRequest;
using CreateRequestMovie = MovieDB.Api.Models.Movies.CreateRequest;
using CreateRequestConcert = MovieDB.Api.Models.Concerts.CreateRequest;
using CreateRequestTheater = MovieDB.Api.Models.Theaters.CreateRequest;

namespace MovieDB.Api.Helpers
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountResponse>();
            CreateMap<Account, AuthenticateResponse>();
            CreateMap<RegisterRequest, Account>();
            CreateMap<UpdateRequestAccount, Account>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop is null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string) prop)) return false;

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
                .ForAllMembers(x => x.Condition((_, _, value) =>
                {
                    if (value is null) return false;
                    if (value.GetType() == typeof(string) && string.IsNullOrEmpty((string) value)) return false;

                    return true;
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
                .ForAllMembers(x => x.Condition((_, _, value) =>
                {
                    if (value is null) return false;
                    if (value.GetType() == typeof(string) && string.IsNullOrEmpty((string) value)) return false;

                    return true;
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
                .ForAllMembers(x => x.Condition((_, _, value) =>
                {
                    if (value is null) return false;
                    if (value.GetType() == typeof(string) && string.IsNullOrEmpty((string) value)) return false;

                    return true;
                }));
        }
    }
}
