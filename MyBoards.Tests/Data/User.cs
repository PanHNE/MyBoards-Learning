using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBoards.Models.Account;

namespace MyBoards.Tests.Data
{
    public static class User
    {
        public static IEnumerable<object[]> GetInvalidDataForRegisterUser()
        {
            var invalidModels = new List<RegisterUserDto>()
            {
                new RegisterUserDto()
                {
                    FullName = "Test User",
                    Email = "testtest.pl",
                    ConfirmEmail = "testtest.pl",
                    Password = "testPassword12#",
                    ConfirmPassword = "testPassword12#"
                },
                new RegisterUserDto()
                {
                    FullName = "Test User",
                    Email = "test@test.pl",
                    ConfirmEmail = "tt@test.pl",
                    Password = "testPassword12#",
                    ConfirmPassword = "testPassword12#"
                },
                new RegisterUserDto()
                {
                    FullName = "Test User",
                    Email = "test@test.pl",
                    ConfirmEmail = "test@test.pl",
                    Password = "tes",
                    ConfirmPassword = "tes"
                },
                new RegisterUserDto()
                {
                    FullName = "Test User",
                    Email = "test@test.pl",
                    ConfirmEmail = "test@test.pl",
                    Password = "testPassword12#",
                    ConfirmPassword = "testPassword"
                }
            };

            return invalidModels.Select( e => new object[] { e } );
        }
    }
}
