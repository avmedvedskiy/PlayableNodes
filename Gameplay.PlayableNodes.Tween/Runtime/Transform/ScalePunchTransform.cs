using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class ScalePunchTransform : TweenAnimation<Transform>
    {
        [SerializeField] private ToFromValue<Vector3> _from;
        [SerializeField] private Vector3 _to = Vector3.one;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _elasticity = 1f;

        protected override Tween GenerateTween()
        {
            Target.localScale = _from.ConvertValue(Target.localScale);
            return Target
                .DOPunchScale(_to, Duration, _vibrato, _elasticity)
                .OnComplete(OnComplete);
        }

        private void OnComplete()
        {
            if (_from.Type == ToFromType.Direct)
                Target.localScale = _from;
        }
    }
}