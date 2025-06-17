using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using FluentValidation;
using FilmAPI.Reposiotories;
using System.Threading;
using FilmAPI.DTOs;

namespace FilmAPI.Validators
{
    public class FilmValidator : AbstractValidator<CreateFilmDTO>
    {
        private readonly IFilmRepository _repo;
        public FilmValidator(IFilmRepository repo)
        {
            _repo = repo;

            ConfigureYearValidation();
            ConfigureTitleValidation();
            ConfigureDirectorValidation();
        }

        private void ConfigureYearValidation()
        {
            RuleFor(film => film.Year)
                .InclusiveBetween(1888, DateTime.Now.Year)
                .WithMessage("Year must be between 1888 and the current year.");
        }

        private void ConfigureTitleValidation()
        {
            RuleFor(film => film.Title)
                .NotNull().WithMessage("Title is required")
                .NotEmpty().WithMessage("Title cannot be empty") ;
                // .Must(title => !_repo.Any(f => f.Title == title))
                // .WithMessage("Title must be unique.");
        }

        private void ConfigureDirectorValidation()
        {
            RuleFor(film => film.Director)
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name can only contain letters and spaces.")
                .When(f => !string.IsNullOrWhiteSpace(f.Director), ApplyConditionTo.CurrentValidator);
        }
    }
}