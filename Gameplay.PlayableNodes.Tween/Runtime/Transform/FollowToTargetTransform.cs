using System;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{

    [Serializable]
    [Description("Moves a Transform to continuously follow a target Transform")]
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