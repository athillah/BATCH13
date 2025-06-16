using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using FluentValidation;
using FilmAPI.Reposiotories;
using System.Threading;

namespace FilmAPI.Validators
{
    public class FilmValidator : AbstractValidator<Film>
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
                .GreaterThanOrEqualTo(1888)
                .WithMessage("Year must be greater than or equal to 1888, the year the first film was made.")
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Year must be less than or equal to the current year.");
        }

        private void ConfigureTitleValidation()
        {
            RuleFor(film => film.Title)
                .NotNull().WithMessage("Title is required")
                .NotEmpty().WithMessage("Title cannot be empty")
                .MustAsync(BeUniqueTitle).WithMessage("Title must be unique.");
        }

        private void ConfigureDirectorValidation() {
            RuleFor(film => film.Director)
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name can only contain letters and spaces.")
                .When(f => !string.IsNullOrWhiteSpace(f.Director), ApplyConditionTo.CurrentValidator);
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken ct)
        {
            return !await _repo
                .AnyAsync(f => f.Title == title, ct);
        }
    }
}