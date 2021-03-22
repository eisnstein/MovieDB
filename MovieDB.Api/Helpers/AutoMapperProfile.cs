using AutoMapper;
using MovieDB.Api.Entities;
using MovieDB.Api.Models.Accounts;

namespace MovieDB.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountResponse>();
            CreateMap<Account, AuthenticateResponse>();
            CreateMap<RegisterRequest, Account>();
        }
    }
}