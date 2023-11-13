using System.Collections.Generic;

namespace PlayableNodes.Extensions
{
    public static class Extensions
    {
        public static float TotalDuration(this IAnimation animation)
        {
            return animation.Duration + animation.Delay;
        }
    }
}