using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using Toph.Domain.Entities;

namespace Toph.UI.Infrastructure
{
    public interface INHibernateSessionFactoryHelper
    {
        ISessionFactory CurrentSessionFactory { get; }
    }

    public class NHibernateSessionFactoryHelper : INHibernateSessionFactoryHelper
    {
        public NHibernateSessionFactoryHelper()
        {
            CurrentSessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey("DefaultConnection")))
                .Mappings(map => map.FluentMappings.AddFromAssemblyOf<UserProfileMap>())
                .BuildSessionFactory();
        }

        public ISessionFactory CurrentSessionFactory { get; private set; }
    }

    public class UserProfileMap : ClassMap<UserProfile>
    {
        public UserProfileMap()
        {
            Id(x => x.Id, "UserId").GeneratedBy.Identity();
            Map(x => x.Username).WithMaxLength().Not.Nullable();
        }
    }

    public static class PropertyPartExtensions
    {
        public static PropertyPart WithMaxLength(this PropertyPart map)
        {
            return map.Length(10000);
        }
    }
}
