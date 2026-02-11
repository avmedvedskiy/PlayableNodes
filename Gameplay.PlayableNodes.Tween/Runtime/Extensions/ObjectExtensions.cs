using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes.Animations
{
    public static class ObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyOrPreview(this Object o)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate(o);
            else
                Object.Destroy(o);
#else
            Object.Destroy(o);
#endif
        }

        private const float DEFAULT_TWEEN_DURATION = 0.33f;
        private const Ease DEFAULT_TWEEN_ACTIVE_EASE = Ease.OutBack;
        private const Ease DEFAULT_TWEEN_DISABLE_EASE = Ease.InBack;

        /// <summary>
        /// Animated activation of gameobject with scale from one to zero
        /// </summary>
        public static Tween DOSetActive(
            this GameObject gameObject,
            bool value,
            float duration = DEFAULT_TWEEN_DURATION)
        {
            if (gameObject.activeSelf == value)
                return null;

            if (value)
                gameObject.SetActive(true);

            var endValue = value ? Vector3.one : Vector3.zero;
            Ease ease = value ? DEFAULT_TWEEN_ACTIVE_EASE : DEFAULT_TWEEN_DISABLE_EASE;
            Tween t = gameObject.transform.DOScale(endValue, duration).SetEase(ease);
            if (!value)
                t.OnComplete(() => gameObject.SetActive(false));

            return t;
        }
    }
}