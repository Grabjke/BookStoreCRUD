using BookStore.Domain.Entities;
using FluentValidation;

namespace BookStore.Validators
{
    public class BookValidator: AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(t=>t.Title)
                .NotEmpty().WithMessage("Укажите название книги")
                .MaximumLength(50).WithMessage("Название книги слишком большое");

            RuleFor(d => d.Description)
                .NotEmpty().WithMessage("Укажите описание книги")
                .MaximumLength(1000).WithMessage("Описание не должно превышать 1000 символов");

            RuleFor(x => x.Price).NotEmpty()
              .WithMessage("Укажите цену")
              .GreaterThan(0).WithMessage("Цена должна быть больше 0");

            RuleFor(x => x.Pages).NotEmpty()
              .WithMessage("Укажите количество страниц")
              .GreaterThan(0).WithMessage("Количество страниц должно быть больше 0");




        }
    }
}
