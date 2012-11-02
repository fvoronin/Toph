using System;
using Toph.Common.DataAccess;
using Toph.Domain.Entities;

namespace Toph.Domain.Services
{
    public interface IUserService
    {
        UserProfile GetUserProfile(string userName);
        UserProfile AddUserProfile(string userName);
    }

    public class UserService : IUserService
    {
        public UserService(IRepository repository)
        {
            _repository = repository;
        }

        private readonly IRepository _repository;

        public UserProfile GetUserProfile(string userName)
        {
            return _repository.Get<UserProfile>(x => x.Username == userName);
        }

        public UserProfile AddUserProfile(string userName)
        {
            var userProfile = new UserProfile(userName);

            _repository.Add(userProfile);

            return userProfile;
        }
    }
}
