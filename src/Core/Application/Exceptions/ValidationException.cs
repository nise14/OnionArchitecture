using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    private const string GenericError = "One or more validation errors have occured";

    public List<string> Errors { get; }

    public ValidationException() : base(GenericError)
    {
        Errors = new List<string>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors.AddRange(failures.Select(x => x.ErrorMessage));
    }
}