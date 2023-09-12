using FluentValidation;
using MyBoards.Entities;
using MyBoards.Models.Account;

namespace MyBoards.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(MyBoardsContext myBoardContext)
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.Email)
                .Custom((value, context) =>
                {
                    var emailInuse = myBoardContext.Users.Any( u => u.Email == value );

                    if (emailInuse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });

            RuleFor(u => u.ConfirmEmail)
                .NotEmpty()
                .Equal(u => u.Email);

            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(u => u.ConfirmPassword)
                .NotEmpty()
                .Equal(u => u.Password);
        }
    }
}
