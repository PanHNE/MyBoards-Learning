using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyBoards.Entities;
using MyBoards.Models.Account;

namespace MyBoards.Services
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string GeneraterJwt(LoginUserDto loginUserDto);
    }
    public class UserService : IUserService
    {
        private readonly MyBoardsContext _myBoardsContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationsSettings _authenticationSettings;

        public UserService(MyBoardsContext myBoardsContext, IMapper mapper, IPasswordHasher<User> passwordHasher, AuthenticationsSettings authenticationsSettings)
        {
            _myBoardsContext = myBoardsContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationsSettings;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = new User()
            {
                FullName = registerUserDto.FullName,
                Email = registerUserDto.Email,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

            newUser.Password = hashedPassword;

            _myBoardsContext.Add(newUser);
            _myBoardsContext.SaveChanges();
        }

        public string Login(LoginUserDto loginUserDto)
        {
            var user = _myBoardsContext.Users.FirstOrDefault(u => u.Email == loginUserDto.Email);

            if (user is null) { throw new Exception("Wrong email"); }

            var isPasswordCorect = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

            if (user is null) { throw new Exception("Wrong password"); }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.JwtIssuer,
                audience: _authenticationSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
