using System.Collections.Generic;

namespace Intro.Application.Validation
{
    public class IntroError
    {
        public IntroError()
        {
            this.Messages = new List<string>();
        }

        public string Name { get; set; }

        public IList<string> Messages { get; set; }
    }
}