using System;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class LookAtTargetTransform : TweenAnimation<Transform>, IChangeEndValue<Transform>
    {
        [SerializeField] private MoveSpace _moveSpace;
        [SerializeField] private Transform _to;

        protected override Tween GenerateTween()
        {
            return Target
                .DOLookAt(_to.position, Duration);
        }

        public void ChangeEndValue(Transform value)
        {
            _to = value;
        }
    }
}