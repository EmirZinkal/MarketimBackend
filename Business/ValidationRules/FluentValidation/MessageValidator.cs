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
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(m => m.MessageText)
            .NotEmpty().WithMessage(Messages.MessageContentNotEmpty)
            .MaximumLength(500).WithMessage(Messages.MessageContentMaxLength);

            RuleFor(m => m.SenderId)
                .GreaterThan(0).WithMessage(Messages.MessageSenderIdRequired);

            RuleFor(m => m.ReceiverId)
                .GreaterThan(0).WithMessage(Messages.MessageReceiverIdRequired);
        }
    }
}
