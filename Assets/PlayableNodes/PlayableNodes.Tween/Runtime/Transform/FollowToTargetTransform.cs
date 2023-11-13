using System;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class FollowToTargetTransform : TweenAnimation<Transform>
    {
        [SerializeField] private Transform _endTarget;

        protected override Tweener GenerateTween() => Target
            .DOFollowTarget(_endTarget, Duration);
    }
}