using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using FluentValidation;

namespace FilmAPI.Validators
{
    public class ReviewValidator : AbstractValidator<Review>
    {
        public ReviewValidator()
        {
            ConfigureContentValidation();
            ConfigureRateValidation();
        }

        private void ConfigureRateValidation()
        {
            RuleFor(review => review.Rate)
                .InclusiveBetween(0, 10)
                .WithMessage("Rate must be between 0 and 10.");
        }

        private void ConfigureContentValidation()
        {
            RuleFor(review => review.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(500)
                .WithMessage("Content must not exceed 500 characters.");
        }
    }
}