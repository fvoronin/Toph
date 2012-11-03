using System;
using Toph.Common.DataAccess;

namespace Toph.Domain.Commands
{
    public interface IDomainCommand<out TResult> where TResult : DomainCommandResult
    {
        TResult Execute(IRepository repository);
    }

    public interface IDomainCommandHandler
    {
        TResult Execute<TResult>(IDomainCommand<TResult> command) where TResult : DomainCommandResult;
    }

    public class DomainCommandHandler : IDomainCommandHandler
    {
        public DomainCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        private readonly IRepository _repository;

        public TResult Execute<TResult>(IDomainCommand<TResult> command) where TResult : DomainCommandResult
        {
            return command.Execute(_repository);
        }
    }
}
