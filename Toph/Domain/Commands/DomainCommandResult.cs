using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toph.Common;

namespace Toph.Domain
{
    public class DomainCommandResult
    {
        public DomainCommandResult()
        {
        }

        public DomainCommandResult(string error) : this("", error)
        {
        }

        public DomainCommandResult(string key, string error)
        {
            Add(new DomainCommandError(key, error));
        }

        private readonly IList<DomainCommandError> _errors = new List<DomainCommandError>();

        public IReadOnlyList<DomainCommandError> Errors
        {
            get { return _errors.AsReadOnly(); }
        }

        public void Add(DomainCommandError error)
        {
            _errors.Add(error);
        }

        public bool AnyErrors()
        {
            return _errors.Any();
        }

        public bool NoErrors()
        {
            return !AnyErrors();
        }
    }

    public class DomainCommandError
    {
        public DomainCommandError(string key, string error)
        {
            Key = key ?? "";
            Error = error ?? "Unknown error";
        }

        public string Key { get; private set; }
        public string Error { get; private set; }
    }

    public static class DomainCommandResultExtensions
    {
        public static T Add<T>(this T result, string error) where T : DomainCommandResult
        {
            result.Add(new DomainCommandError("", error));

            return result;
        }

        public static T Add<T>(this T result, string key, string error) where T : DomainCommandResult
        {
            result.Add(new DomainCommandError(key, error));

            return result;
        }

        public static T Add<T>(this T result, IEnumerable<ValidationResult> validationResults) where T : DomainCommandResult
        {
            foreach (var validationResult in validationResults)
                result.Add(validationResult.MemberNames.Join(", "), validationResult.ErrorMessage);

            return result;
        }

        public static T AddValidationErrors<T>(this T result, object instance) where T : DomainCommandResult
        {
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(instance, new ValidationContext(instance), validationResults, true);

            return result.Add(validationResults);
        }
    }
}
