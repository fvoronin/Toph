using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Toph.Common;

namespace Toph.Domain.Services
{
    public interface IValidationFacade
    {
        ServiceResult Validate(object instance);
    }

    public class ValidationFacade : IValidationFacade
    {
        public ServiceResult Validate(object instance)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(instance, new ValidationContext(instance), validationResults, true);

            var serviceResult = new ServiceResult();

            foreach (var validationResult in validationResults)
                serviceResult.Add(validationResult.MemberNames.Join(", "), validationResult.ErrorMessage);

            return serviceResult;
        }
    }
}
