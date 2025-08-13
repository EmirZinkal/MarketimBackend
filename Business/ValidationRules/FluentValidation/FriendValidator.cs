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
    public class FriendValidator : AbstractValidator<Friend>
    {
        public FriendValidator()
        {
            RuleFor(f => f.UserId)
            .GreaterThan(0).WithMessage(Messages.FriendUserIdRequired);

            RuleFor(f => f.FriendId)
                .GreaterThan(0).WithMessage(Messages.FriendFriendIdRequired);
        }
    }
}
