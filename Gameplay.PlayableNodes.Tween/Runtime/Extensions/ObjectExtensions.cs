using System.Runtime.CompilerServices;
using UnityEngine;

namespace PlayableNodes.Animations
{
    public static class ObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyOrPreview(this Object o)
        {
#if UNITY_EDITOR
            if(!Application.isPlaying)
                Object.DestroyImmediate(o);
            else
                Object.Destroy(o);
#else
            Object.Destroy(o);
#endif
        }
    }
}