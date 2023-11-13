using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class ScaleTransform : TweenAnimation<Transform>
    {
        [SerializeField] private ToFromValue<float> _from;
        [SerializeField] private ToFromValue<float> _to;

        protected override Tweener GenerateTween() => Target
            .DOScale(_to, Duration)
            .ChangeValuesVector(_to, _from);
    }
}