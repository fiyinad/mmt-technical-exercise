using System;
using System.Linq;
using System.Text.RegularExpressions;
using Customers.Models.Requests;
using FluentValidation;

namespace Customers.Validators
{
    public class CustomerOrderRequestValidator : AbstractValidator<CustomerOrderRequest>
    {
        public CustomerOrderRequestValidator()
        {
          RuleFor(x => x.User)
            .NotNull()
            .NotEmpty()
            .Must(x => NotBeWhiteSpace(x))
              .WithMessage(x => $"The parameter '{nameof(x.User)}' must not be whitespace")
            .Must(x => BeValidEmail(x))
              .WithMessage(x => $"The parameter '{nameof(x.User)}' must be a valid email");
          
          RuleFor(x => x.CustomerId)
            .NotNull()
            .NotEmpty()
            .Must(x => NotBeWhiteSpace(x))
              .WithMessage(x => $"The parameter '{nameof(x.CustomerId)}' must not be whitespace");
        }

        private bool NotBeWhiteSpace(string str)
        {
          return null != str && !str.All(char.IsWhiteSpace);
        }

        private bool BeValidEmail(string email)
        {
          try
            {
              return Regex.IsMatch(email,
                  @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                  RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
              return false;
            }
        }
    }
}