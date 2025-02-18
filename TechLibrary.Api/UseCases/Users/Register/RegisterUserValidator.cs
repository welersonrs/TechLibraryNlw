using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Users.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestUserJson>
    {
        public RegisterUserValidator() 
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("The name cannot be empty.");
            RuleFor(request => request.Email).EmailAddress().WithMessage("The email is not valid.");
            RuleFor(request => request.Password).NotEmpty().WithMessage("The password is mandatory.");
            When(request => string.IsNullOrEmpty(request.Password) == false, () =>
            {
                RuleFor(request => request.Password.Length).GreaterThanOrEqualTo(6).WithMessage("Password may have 6 or more characters.");
            });
        }
    }
}
