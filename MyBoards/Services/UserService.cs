using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyBoards.Entities;
using MyBoards.Models.Account;

namespace MyBoards.Services
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
    }
    public class UserService : IUserService
    {
        private readonly MyBoardsContext _myBoardsContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(MyBoardsContext myBoardsContext, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _myBoardsContext = myBoardsContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
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
    }
}
