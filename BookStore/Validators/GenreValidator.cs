using BookStore.Domain.Entities;
using FluentValidation;

namespace BookStore.Validators
{
    public class GenreValidator: AbstractValidator<Genre>
    {
        public GenreValidator()
        {
            RuleFor(x=>x.Title)
                .NotEmpty().WithMessage("Заполните название жанра");
        }
    }
}
