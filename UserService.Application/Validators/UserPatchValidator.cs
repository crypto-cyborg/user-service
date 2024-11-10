using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Data.Dtos;

namespace UserService.Application.Validators
{

    public class UserPatchValidator : AbstractValidator<UserPatchDto>
    {
        public UserPatchValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Username), () =>
            {
                RuleFor(x => x.Username)
                    .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                    .MaximumLength(50).WithMessage("Username must be less than 50 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("Invalid email format.")
                    .MaximumLength(100).WithMessage("Email must be less than 100 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                    .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
                    .MaximumLength(50).WithMessage("First name must be less than 50 characters.");
            });

            When(x => !string.IsNullOrEmpty(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                    .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
                    .MaximumLength(50).WithMessage("Last name must be less than 50 characters.");
            });
        }
    }

}
