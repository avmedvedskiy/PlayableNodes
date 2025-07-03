using System;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Scales the Transform using animation curves for each axis")]
    public class ScaleByCurveTransform : TweenAnimation<Transform>
    {
        [Tooltip("When disabled will be used X axes as main")]
        [SerializeField] private bool _separateAxes = false;
        [SerializeField] private AnimationCurve _x = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        [SerializeField] private AnimationCurve _y = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        [SerializeField] private AnimationCurve _z = AnimationCurve.Linear(0f, 1f, 1f, 1f);

        protected override Tween GenerateTween()
        {
            return _separateAxes
                ? Target.DOEvaluateScaleByCurve(_x, _y, _z, Duration)
                : Target.DOEvaluateScaleByCurve(_x, Duration);
        }
    }
}