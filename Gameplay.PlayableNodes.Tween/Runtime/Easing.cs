using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Internal;

namespace PlayableNodes
{
    public static class EasingTweenExtension
    {
        public static T SetEase<T>(this T t, Easing easing) where T : Tween => easing.SetEase(t);
    }

    [Serializable]
    public struct Easing
    {
        [SerializeField] private Ease _ease;

        [SerializeField] private AnimationCurve _curve;

        [SerializeField] private float _scale;

        public Easing(Ease ease, float scale)
        {
            _scale = scale;
            _ease = ease;
            _curve = null;
        }

        public Easing(AnimationCurve curve, float scale)
        {
            _ease = Ease.INTERNAL_Custom;
            _scale = scale;
            _curve = curve;
        }

        public static Easing Default => new(Ease.OutBack, 1f);

        public T SetEase<T>(T t) where T : Tween
        {
            if (t is not { active: true })
                return t;

            if (_ease == Ease.INTERNAL_Custom)
                t.SetEase(_curve);
            else
                t.SetEase(_ease);
            
            t.easeOvershootOrAmplitude *= _scale;
            return t;
        }
    }
}