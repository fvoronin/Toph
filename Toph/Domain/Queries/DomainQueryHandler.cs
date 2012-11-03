using System;
using Toph.Common.DataAccess;

namespace Toph.Domain.Queries
{
    public interface IDomainQuery<out TResult>
    {
        TResult Execute(IRepository repository);
    }

    public interface IDomainQueryHandler
    {
        TResult Execute<TResult>(IDomainQuery<TResult> query);
    }

    public class DomainQueryHandler : IDomainQueryHandler
    {
        public DomainQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        private readonly IRepository _repository;

        public TResult Execute<TResult>(IDomainQuery<TResult> query)
        {
            return query.Execute(_repository);
        }
    }
}
