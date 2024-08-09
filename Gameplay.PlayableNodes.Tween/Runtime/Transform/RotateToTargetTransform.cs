using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class RotateToTargetTransform : TweenAnimation<Transform>, IChangeEndValue<Transform>
    {
        [SerializeField] private MoveSpace _moveSpace;
        [SerializeField] private Transform _to;

        protected override Tween GenerateTween()
        {
            return Target.DORotateQuaternion(_moveSpace, _to, Duration);
        }

        public void ChangeEndValue(Transform value)
        {
            _to = value;
        }
    }
}