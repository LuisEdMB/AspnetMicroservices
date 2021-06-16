using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(g => g.PropertyName, g => g.ErrorMessage)
                .ToDictionary(failGroup => failGroup.Key, failGroup => failGroup.ToArray());
        }

        public ValidationException() : base("One or more validation failures have ocurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
    }
}
