using BookStore.Domain.Entities;
using FluentValidation;

namespace BookStore.Validators
{
    public class AuthorValidator: AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(x=>x.Name)
                .NotEmpty().WithMessage("Введите имя автора");
        }
    }
}
