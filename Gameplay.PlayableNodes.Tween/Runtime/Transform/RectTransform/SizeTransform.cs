using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Tweens sizeDelta of a RectTransform between two values")]
    public class SizeTransform : TweenAnimation<RectTransform>
    {
        [SerializeField] private ToFromValue<Vector2> _from ;
        [SerializeField] private ToFromValue<Vector2> _to;
        [SerializeField] private AxisConstraint _axisConstraint = AxisConstraint.None;

        protected override Tween GenerateTween() =>
            Target
                .DOSizeDelta(_to, Duration)
                .SetOptions(_axisConstraint)
                .ChangeValuesVector(_to, _from);

    }
}