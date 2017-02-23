using Intro.Application.Validation;
using Intro.Domain.PersistentModels;
using Nancy;
using Nancy.Responses.Negotiation;

namespace Intro.NancyExtensions
{
    public static class NancyModuleExtensions
    {
        public static void EnableCors(this NancyModule module)
        {
            module.After.AddItemToEndOfPipeline(x => x.Response.WithHeader("Access-Control-Allow-Origin", "*"));
        }

        public static Negotiator RespondWith(this NancyModule nancyModule, IValidationResult result)
        {
            return nancyModule.Negotiate.WithStatusCode(StatusCode(result)).WithModel(result);
        }

        public static Negotiator RespondWith(this NancyModule nancyModule, IEntityValidationResult result)
        {
            return nancyModule.Negotiate.WithStatusCode(StatusCode(result)).WithModel(result);
        }

        public static Negotiator RespondWith<T>(this NancyModule nancyModule, IEntityValidationResult<T> result) where T : Entity
        {
            return nancyModule.Negotiate.WithStatusCode(StatusCode(result)).WithModel(result);
        }

        private static HttpStatusCode StatusCode(IValidationResult result)
        {
            return result.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        }
    }
}