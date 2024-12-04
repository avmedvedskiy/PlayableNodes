using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class SizeByTargetTransform : TweenAnimation<RectTransform>, IChangeEndValue<Transform>
    {
        [SerializeField] private ToFromValue<Vector2> _from ;
        [SerializeField] private Transform _to;
        [SerializeField] private Vector2 _additionalSize;
        [SerializeField] private AxisConstraint _axisConstraint = AxisConstraint.None;

        private RectTransform To =>(RectTransform)_to;

        protected override Tween GenerateTween() =>
            Target
                .DOSizeDelta(To.sizeDelta + _additionalSize, Duration)
                .SetOptions(_axisConstraint)
                .ChangeValuesVector(To.sizeDelta + _additionalSize, _from);
        

        public void ChangeEndValue(Transform value) => _to = value;
    }
}