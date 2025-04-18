using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class ScaleTransform : TweenAnimation<Transform>, IChangeEndValue<float>
    {
        [SerializeField] private ToFromValue<float> _from = ToFromValue<float>.Dynamic;
        [SerializeField] private ToFromValue<float> _to;

        protected override Tween GenerateTween() =>
            Target
                .DOScale(_to, Duration)
                .ChangeValuesVectorOnStart(_to, _from);

        public void ChangeEndValue(float value)
        {
            _to = new ToFromValue<float>(value);
        }
    }
}