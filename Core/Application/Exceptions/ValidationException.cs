using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public List<string> Errors;

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }

    public ValidationException(List<ValidationFailure> failures) : this()
    {
        foreach (var item in failures) Errors.Add(item.ErrorMessage);
    }
}