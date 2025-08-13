using Business.Constants;
using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class ShoppingListValidator : AbstractValidator<ShoppingList>
    {
        public ShoppingListValidator()
        {
            RuleFor(s => s.Title)
            .NotEmpty().WithMessage(Messages.ShoppingListTitleNotEmpty);

            RuleFor(s => s.UserId)
                .GreaterThan(0).WithMessage(Messages.ShoppingListUserIdRequired);
        }
    }
}
