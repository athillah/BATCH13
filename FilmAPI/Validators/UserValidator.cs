using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using FluentValidation;

namespace FilmAPI.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            ConfigureFirstNameValidation();
            ConfigureLastNameValidation();
            ConfigureUserNameValidation();
        }

        private void ConfigureFirstNameValidation()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("Student name is required.")
                .Length(1, 50)
                .WithMessage("First name must be between 1 and 50 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name can only contain letters and spaces.");
        }

        private void ConfigureLastNameValidation()
        {
            RuleFor(user => user.FirstName)
                .MaximumLength(50)
                .WithMessage("Last name must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name can only contain letters and spaces.");
        }

        private void ConfigureUserNameValidation()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("Student name is required.")
                .Length(5, 25)
                .WithMessage("User name must be between 5 and 25 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name can only contain letters and spaces.");
        }
    }
}