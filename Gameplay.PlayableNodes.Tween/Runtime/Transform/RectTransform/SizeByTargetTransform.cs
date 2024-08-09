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

        private RectTransform To =>(RectTransform)_to;

        protected override Tween GenerateTween() =>
            Target
                .DOSizeDelta(To.sizeDelta, Duration)
                .ChangeValuesVector(To.sizeDelta, _from);
        

        public void ChangeEndValue(Transform value) => _to = value;
    }
}