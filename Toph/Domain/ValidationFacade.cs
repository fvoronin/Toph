using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Toph.Common;

namespace Toph.Domain
{
    public interface IValidationFacade
    {
        CommandResult Validate(object instance);
    }

    public class ValidationFacade : IValidationFacade
    {
        public CommandResult Validate(object instance)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(instance, new ValidationContext(instance), validationResults, true);

            var result = new CommandResult();

            foreach (var validationResult in validationResults)
                result.Add(validationResult.MemberNames.Join(", "), validationResult.ErrorMessage);

            return result;
        }
    }
}
