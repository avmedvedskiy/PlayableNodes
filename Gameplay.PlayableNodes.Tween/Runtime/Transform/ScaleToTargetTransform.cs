using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class ScaleToTargetTransform : TweenAnimation<Transform>, IChangeEndValue<Transform>
    {
        [SerializeField] private ToFromValue<Vector3> _from = ToFromValue<Vector3>.Dynamic;
        [SerializeField] private Transform _to;

        protected override Tween GenerateTween() => Target
            .DOScale(_to.localScale, Duration)
            .ChangeValuesVectorOnStart(_to.localScale, _from);

        public void ChangeEndValue(Transform value)
        {
            _to = value;
        }
    }
}