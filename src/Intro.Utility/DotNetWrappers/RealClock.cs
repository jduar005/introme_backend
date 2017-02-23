using System;

namespace Intro.Utility.DotNetWrappers
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }

    // wraps Clock so that we can easily test timestamp logic
    public class RealClock : IClock
    {
        public DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}