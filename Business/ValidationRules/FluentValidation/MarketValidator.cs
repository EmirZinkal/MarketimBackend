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
    public class MarketValidator:AbstractValidator<Market>
    {
        public MarketValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage(Messages.MarketNameNotEmpty)
                .MinimumLength(2).WithMessage(Messages.MarketNameMinLength);
        }
    }
}
