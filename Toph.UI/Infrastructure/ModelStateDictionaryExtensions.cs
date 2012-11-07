using System;
using System.Web.Mvc;
using Toph.Domain.Services;

namespace Toph.UI
{
    public static class ModelStateDictionaryExtensions
    {
        public static ModelStateDictionary AddModelErrors(this ModelStateDictionary modelState, ServiceResult result)
        {
            foreach (var error in result.Errors)
                modelState.AddModelError(error.Key, error.Error);

            return modelState;
        }
    }
}
