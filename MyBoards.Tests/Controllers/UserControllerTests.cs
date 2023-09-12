
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyBoards.Entities;
using MyBoards.Models.Account;
using MyBoards.Services;
using MyBoards.Tests.Helpers;
using Task = System.Threading.Tasks.Task;
using User = MyBoards.Tests.Data.User;

namespace MyBoards.Tests.Controllers
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        public static IEnumerable<object[]> invalidDataForRegisterUserDto = User.GetInvalidDataForRegisterUser();

        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly Mock<IUserService> _mockUSerService = new Mock<IUserService>();

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _applicationFactory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<MyBoardsContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IUserService>(_mockUSerService.Object);

                        services.AddDbContext<MyBoardsContext>(options => options.UseInMemoryDatabase("MyBoardDbTest"));
                    });
                });
            _httpClient = _applicationFactory.CreateClient();
        }

        [Fact]
        public async Task Register_WithValidModel_ReturnCreated()
        {
            // arrange
            var newUser = new RegisterUserDto()
            {
                FullName = "Test User",
                Email = "testowy@test.pl",
                ConfirmEmail = "testowy@test.pl",
                Password = "testPassword12#",
                ConfirmPassword = "testPassword12#"
            };

            var httpContent = newUser.ToJsonHttpContent();

            // act
            var response = await _httpClient.PostAsync("api/user/register", httpContent);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Theory]
        [MemberData(nameof(invalidDataForRegisterUserDto))]
        public async Task Register_WithInvalidModel_ReturnBadRequest(RegisterUserDto registerUserDto)
        {
            // arrange
            var httpContent = registerUserDto.ToJsonHttpContent();

            // act
            var response = await _httpClient.PostAsync("api/user/register", httpContent);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
