using Intro.Application.Services;
using Intro.Application.Validation;
using Intro.Domain.PersistentModels;
using Intro.Domain.ViewModels;
using Intro.NancyExtensions;
using Intro.Security;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.ModelBinding;
using System;
using System.Text;

namespace Intro.Modules
{
    // TODO Nancy Test
    public class AuthModule : NancyModule
    {
        public AuthModule(ITokenizer tokenizer, IAuthService authService, IUserIdentityAdaptorFactory userIdentityAdaptorFactory, Func<IValidationResult> validationResultFactory)
            : base("/auth")
        {
            Post["/"] = parameters =>
            {
                var authenticationRequest = this.Bind<AuthenticationRequest>();
                var passwordBytes = Convert.FromBase64String(authenticationRequest.Password);
                var password = Encoding.UTF8.GetString(passwordBytes);
                var identity = authService.Authenticate(authenticationRequest.UserName, password);

                if (identity == null)
                {
                    var validationResult = validationResultFactory();
                    validationResult.AddError("Login", "Login.Invalid");

                    return this.RespondWith(validationResult);
                }

                var userIdentity = userIdentityAdaptorFactory.FromUser(identity);
                var token = tokenizer.Tokenize(userIdentity, Context);

                var authorization = new AuthorizationId
                                    {
                                        Token = token
                                    };

                authService.Store(authorization);

                return authorization;
            };

            Post["/logout"] = parameters =>
            {
                var token = this.Request.Headers.Authorization.Replace("Token ", string.Empty);
                var authorization = new AuthorizationId { Token = token };
                var result = authService.Logout(authorization);

                return this.RespondWith(result);
            };
        }
    }
}