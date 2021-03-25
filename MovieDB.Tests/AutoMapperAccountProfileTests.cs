using System;
using AutoMapper;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Accounts;
using MovieDB.Api.Models.Movies;
using MovieDB.Tests.Factories;
using Xunit;
using UpdateRequest = MovieDB.Api.Models.Movies.UpdateRequest;

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
            Assert.Equal(account.Title, response.Title);
            Assert.Equal(account.FirstName, response.FirstName);
            Assert.Equal(account.LastName, response.LastName);
            Assert.Equal(account.Email, response.Email);
            Assert.Equal(account.Role, response.Role);
        }
    }
}
