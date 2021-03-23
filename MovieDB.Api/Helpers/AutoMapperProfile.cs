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
            CreateMap<UpdateRequest, Account>()
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
}