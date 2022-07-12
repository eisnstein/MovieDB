using AutoMapper;
using AutoMapper.Internal;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Http.Responses;
using MovieDB.Tests.Factories;

namespace MovieDB.Tests.Unit;

public class AutoMapperAccountProfileTests
{
    private readonly IMapper _mapper;

    public AutoMapperAccountProfileTests()
    {
        if (_mapper is null)
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.Internal().MethodMappingEnabled = false;
                cfg.AddProfile(new AccountProfile());
            });

            // mappingConfig.AssertConfigurationIsValid();

            _mapper = mappingConfig.CreateMapper();
        }
    }

    [Fact]
    public void AutoMapper_Account_To_AccountResponse()
    {
        var account = AccountFactory.CreateAccount();

        var response = _mapper.Map<AccountResponse>(account);

        Assert.Equal(account.Id, response.Id);
        Assert.Equal(account.Email, response.Email);
        Assert.Equal("Admin", response.Role);
    }
}
