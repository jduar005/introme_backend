using Nancy;
using Nancy.Responses;

namespace Intro.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get("/", parameters => new RedirectResponse("/index"));

            Get("/index", parameters => View["index"]);
        }
    }
}