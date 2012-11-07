using System;
using System.Collections.Generic;
using System.Linq;
using Toph.Common;

namespace Toph.Domain.Services
{
    public class ServiceResult
    {
        public ServiceResult()
        {
        }

        public ServiceResult(string error) : this("", error)
        {
        }

        public ServiceResult(string key, string error)
        {
            Add(new ServiceError(key, error));
        }

        private readonly IList<ServiceError> _errors = new List<ServiceError>();

        public IReadOnlyList<ServiceError> Errors
        {
            get { return _errors.AsReadOnly(); }
        }

        public void Add(ServiceError error)
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

    public class ServiceError
    {
        public ServiceError(string key, string error)
        {
            Key = key ?? "";
            Error = error ?? "Unknown error";
        }

        public string Key { get; private set; }
        public string Error { get; private set; }
    }

    public static class ServiceResultExtensions
    {
        public static T Add<T>(this T result, string error) where T : ServiceResult
        {
            result.Add(new ServiceError("", error));

            return result;
        }

        public static T Add<T>(this T result, string key, string error) where T : ServiceResult
        {
            result.Add(new ServiceError(key, error));

            return result;
        }
    }
}
