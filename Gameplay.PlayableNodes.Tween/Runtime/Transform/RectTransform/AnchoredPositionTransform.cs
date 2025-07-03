using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Tweens the RectTransform's anchored position between two values")]
    public class AnchoredPositionTransform : TweenAnimation<RectTransform>
    {
        [SerializeField] private ToFromValue<Vector2> _from;
        [SerializeField] private ToFromValue<Vector2> _to;

        protected override Tween GenerateTween() => 
            Target
                .DOAnchorPos(_to, Duration)
                .ChangeValuesVector(_to, _from);
    }
}