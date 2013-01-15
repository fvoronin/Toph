using System;
using System.Data;
using Toph.Common.DataAccess;
using Toph.Domain.Entities;

namespace Toph
{
    public static class TophExtensions
    {
        /// <summary>Intercepts call to regular IRepository.Get to do a concurrency check</summary>
        public static T Get<T>(this IRepository repository, int id, int version) where T : EntityBase
        {
            var entity = repository.Get<T>(id);

            if (entity.Version != version) throw new DBConcurrencyException();

            return entity;
        }
    }
}
