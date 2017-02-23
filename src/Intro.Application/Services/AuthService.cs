using System;
using System.Linq;
using Intro.Application.Validation;
using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using Intro.Utility.Extensions;

namespace Intro.Application.Services
{
    public interface IAuthService
    {
        IUser Authenticate(string userName, string password);

        void Store(AuthorizationId authorization);

        bool Validate(AuthorizationId authorization);

        IValidationResult Logout(AuthorizationId authorization);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;

        private readonly IRepository<AuthorizationId> _authorizationRepository;

        private readonly Func<IValidationResult> _validationResultFactory;

        public AuthService(IUserRepository userRepository, IRepository<AuthorizationId> authorizationRepository, Func<IValidationResult> validationResultFactory)
        {
            this.userRepository = userRepository;
            this._authorizationRepository = authorizationRepository;
            this._validationResultFactory = validationResultFactory;
        }

        public IUser Authenticate(string userName, string password)
        {
            var user = userRepository.GetByUserName(userName);
            if (user == null)
            {
                return null;
            }

            return this.IsMatchingPassword(user.EncryptedPassword, password) ? user : null;
        }

        public bool Validate(AuthorizationId authorization)
        {
            return _authorizationRepository.Exists(auth => auth.Token == authorization.Token);
        }

        public IValidationResult Logout(AuthorizationId authorization)
        {
            _authorizationRepository.Delete(auth => auth.Token == authorization.Token);

            return _validationResultFactory();
        }

        public void Store(AuthorizationId authorization)
        {
            _authorizationRepository.Add(authorization);
        }

        private bool IsMatchingPassword(byte[] userEncryptedPassword, string password)
        {
            return userEncryptedPassword.SequenceEqual(password.Encrypt());
        }
    }
}