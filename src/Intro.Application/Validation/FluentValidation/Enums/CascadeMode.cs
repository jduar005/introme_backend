using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro.Application.Validation.FluentValidation.Enums
{
    /// <summary>
    /// Specifies how rules should cascade when one fails.
    /// </summary>
    public enum CascadeMode
    {
        /// <summary>
        /// When a rule fails, execution continues to the next rule.
        /// </summary>
        Continue,
        /// <summary>
        /// When a rule fails, validation is stopped and all other rules in the chain will not be executed.
        /// </summary>
        StopOnFirstFailure
    }
}
