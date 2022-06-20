using FinancialChat.WebApi.Domain.Commands;
using FluentValidation;

namespace FinancialChat.WebApi.Domain.Validations;

public class MessageValidator : AbstractValidator<NewMessage>
{
    public MessageValidator()
    {
        RuleFor(p => p.Text).NotEmpty();
        RuleFor(p => p.Username).NotEmpty().EmailAddress();
    }
}
