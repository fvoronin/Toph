using System;
using System.Collections.Generic;
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
            Add(key, error);
        }

        private readonly IList<DomainCommandError> _errors = new List<DomainCommandError>();

        public IReadOnlyList<DomainCommandError> Errors
        {
            get { return _errors.AsReadOnly(); }
        }

        public DomainCommandResult Add(string error)
        {
            return Add("", error);
        }

        public DomainCommandResult Add(string key, string error)
        {
            _errors.Add(new DomainCommandError(key, error));
            return this;
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
}
