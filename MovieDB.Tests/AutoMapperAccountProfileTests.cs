using AutoMapper;
using MovieDB.Api.App.Helpers;
using MovieDB.Shared.Models.Accounts;
using MovieDB.Tests.Factories;
using Xunit;

namespace MovieDB.Tests
{
    public class AutoMapperAccountProfileTests
    {
        private readonly IMapper _mapper;

        public AutoMapperAccountProfileTests()
        {
            if (_mapper is null)
            {
                var mappingConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new AccountProfile());
                });

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
}
