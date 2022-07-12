using AutoMapper;
using AutoMapper.Internal;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Http.Responses;
using MovieDB.Api.App.Models;

namespace MovieDB.Api.App.Helpers;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        this.Internal().MethodMappingEnabled = false;
        CreateMap<Account, AccountResponse>();
        CreateMap<Account, AuthenticateResponse>();
        CreateMap<RegisterRequest, Account>();
        CreateMap<AccountUpdateRequest, Account>()
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
        CreateMap<MovieCreateRequest, Movie>();
        CreateMap<Movie, MovieResponse>();
        CreateMap<MovieUpdateRequest, Movie>()
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
        CreateMap<ConcertCreateRequest, Concert>();
        CreateMap<Concert, ConcertResponse>();
        CreateMap<ConcertUpdateRequest, Concert>()
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
        CreateMap<TheaterCreateRequest, Theater>();
        CreateMap<Theater, TheaterResponse>();
        CreateMap<TheaterUpdateRequest, Theater>()
            .ForAllMembers(x => x.Condition((_, _, value) => value switch
            {
                null => false,
                string v when string.IsNullOrEmpty(v) => false,
                _ => true
            }));
    }
}
