using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    public static class TweenValuesExtensions
    {
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ConvertValue<T>(this ToFromValue<T> toFromValue, T currentValue) =>
            toFromValue.Type == ToFromType.Direct
                ? toFromValue
                : currentValue;

        /// <summary>
        /// Change Start End value by alpha color
        /// </summary>
        public static Tweener ChangeValuesAlpha(this Tweener tween, ToFromValue<float> to,  ToFromValue<float> from)
        {
            var twCore = (TweenerCore<Color, Color, ColorOptions>)tween;
            return twCore.ChangeValues(
                new Color(0f, 0f, 0f, from.ConvertValue(twCore.getter().a)),
                new Color(0f, 0f, 0f, to.ConvertValue(twCore.getter().a)));
        }

        public static Tweener ChangeValuesFloat(this Tweener tween,  ToFromValue<float> to,  ToFromValue<float> from)
        {
            var twCore = (TweenerCore<float, float, FloatOptions>)tween;
            return twCore.ChangeValues(
                from.ConvertValue(twCore.getter()),
                to.ConvertValue(twCore.getter()));
        }

        /// <summary>
        /// Change Vector by float
        /// </summary>
        public static Tweener ChangeValuesVector(this Tweener tween,  ToFromValue<float> to,  ToFromValue<float> from)
        {
            //<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>
            var twCore = (TweenerCore<Vector3, Vector3, VectorOptions>)tween;
            return twCore.ChangeValues(
                from.Type == ToFromType.Direct
                    ? new Vector3(from, from, from)
                    : twCore.getter(),
                to.Type == ToFromType.Direct
                    ? new Vector3(to, to, to)
                    : twCore.getter());
        }

        public static Tweener ChangeValuesVector(this Tweener tween, ToFromValue<Vector3> to, ToFromValue<Vector3> from)
        {
            var twCore = (TweenerCore<Vector3, Vector3, VectorOptions>)tween;
            return twCore.ChangeValues(from.ConvertValue(twCore.getter()), to.ConvertValue(twCore.getter()));
        }
        
        public static Tweener ChangeValuesVector(this Tweener tween, Vector3 to, Vector3 from, float duration = -1f)
        {
            var twCore = (TweenerCore<Vector3, Vector3, VectorOptions>)tween;
            return twCore.ChangeValues(from, to,duration);
        }

        public static Tweener ChangeValuesVector(this Tweener tween,  ToFromValue<Vector2> to, ToFromValue<Vector2> from)
        {
            var twCore = (TweenerCore<Vector2, Vector2, VectorOptions>)tween;
            return twCore.ChangeValues(from.ConvertValue(twCore.getter()), to.ConvertValue(twCore.getter()));
        }
        
        public static Tweener ChangeValuesVector(this Tweener tween, Vector2 to, Vector2 from, float duration = -1f)
        {
            var twCore = (TweenerCore<Vector2, Vector2, VectorOptions>)tween;
            return twCore.ChangeValues(from, to,duration);
        }
        
        public static Tweener ChangeValuesVector(this Tweener tween, Vector2 to, ToFromValue<Vector2> from, float duration = -1f)
        {
            var twCore = (TweenerCore<Vector2, Vector2, VectorOptions>)tween;
            return twCore.ChangeValues(from.ConvertValue(twCore.getter()), to,duration);
        }
        
        /*
        public static Tweener ChangeValuesRotation(this Tweener tween,  ToFromValue<Vector3> to, ToFromValue<Vector3> from)
        {
            var twCore = (TweenerCore<Quaternion, Vector2, QuaternionOptions>)tween;
            return twCore.ChangeValues(from.ConvertValue(twCore.getter()), to.ConvertValue(twCore.getter()));
        }*/


        public static Tweener ChangeValuesColor(this Tweener tween, ToFromValue<Color> to, ToFromValue<Color> from)
        {
            var twCore = (TweenerCore<Color, Color, ColorOptions>)tween;
            return twCore.ChangeValues(from.ConvertValue(twCore.getter()), to.ConvertValue(twCore.getter()));
        }
    }
}