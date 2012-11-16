using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toph.Common.DataAccess;
using Toph.Domain.Entities;

namespace Toph.Domain.Services
{
    public interface IUserService
    {
        ServiceResult Execute(AddUserCommand command);
    }

    public class UserService : IUserService
    {
        public UserService(IRepository repository, IValidationFacade validationFacade)
        {
            _repository = repository;
            _validationFacade = validationFacade;
        }

        private readonly IRepository _repository;
        private readonly IValidationFacade _validationFacade;

        public ServiceResult Execute(AddUserCommand command)
        {
            var serviceResult = _validationFacade.Validate(command);

            if (serviceResult.AnyErrors())
                return serviceResult;

            if (_repository.Find<UserProfile>().Any(x => x.Username == command.Username))
                return serviceResult.Add("Username", "Username already exists. Please enter a different username.");

            _repository.Add(new UserProfile(command.Username));

            return serviceResult;
        }
    }

    public class AddUserCommand
    {
        [Required, RegularExpression(@"^[A-Za-z]+[A-Za-z0-9-]*$", ErrorMessage = "Username may only contain alphanumeric characters or dashes and cannot begin with a dash or number")]
        public string Username { get; set; }
    }
}
