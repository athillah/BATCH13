using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.DTOs;
using FilmAPI.Models;
using FluentValidation;

namespace FilmAPI.Validators
{
    public class ReviewValidator : AbstractValidator<CreateReviewDTO>
    {
        public ReviewValidator()
        {
            // ConfigureUserIdValidation();
            // ConfigureFilmIdValidation();
            ConfigureContentValidation();
            ConfigureRateValidation();
        }

        // private void ConfigureUserIdValidation()
        // {
        //     RuleFor(review => review.UserId)
        //         .NotEmpty()
        //         .WithMessage("User Id is required.");
        // }

        // private void ConfigureFilmIdValidation()
        // {
        //     RuleFor(review => review.FilmId)
        //         .NotEmpty()
        //         .WithMessage("Film Id is required.")
        //         .GreaterThan(0)
        //         .WithMessage("Film Id must be greater than 0.");
        // }

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