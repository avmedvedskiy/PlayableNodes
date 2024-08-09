using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class PositionPunchTransform : TweenAnimation<Transform>
    {
        [SerializeField] private ToFromValue<Vector3> _from = ToFromValue<Vector3>.Dynamic;
        [SerializeField] private Vector3 _to = Vector3.one;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _elasticity = 1f;

        protected override Tween GenerateTween()
        {
            var startPosition = _from.ConvertValue(Target.localPosition);
            Target.localPosition = startPosition;
            return Target
                .DOPunchPosition(_to, Duration, _vibrato, _elasticity)
                .OnStart(()=> startPosition = _from.ConvertValue(Target.localPosition))
                .OnComplete(()=> Target.localPosition = startPosition);
        }
    }
}