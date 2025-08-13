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
    public class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
            .NotEmpty().WithMessage(Messages.ProductNameNotEmpty)
            .MinimumLength(2).WithMessage(Messages.ProductNameMinLength);

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage(Messages.ProductPriceGreaterThanZero);

            RuleFor(p => p.MarketId)
                .GreaterThan(0).WithMessage(Messages.ProductMarketIdRequired);
        }
    }
}
