using System;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    
    [Serializable]
    public class FollowToTargetTransform : TweenAnimation<Transform>, IChangeEndValue<Transform>
    {
        [SerializeField] private Transform _endTarget;

        protected override Tween GenerateTween() => Target
            .DOFollowTarget(_endTarget, Duration);

        public void ChangeEndValue(Transform value)
        {
            _endTarget = value;
        }
    }
}