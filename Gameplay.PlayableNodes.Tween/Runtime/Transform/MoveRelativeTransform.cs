using System;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable] public class MoveRelativeTransform : TweenAnimation<Transform>
    {
        [SerializeField] private Vector3 _direction;
        [SerializeField] private MoveSpace _moveSpace;
        
        protected override Tween GenerateTween()
        {
            return Target
                .DOMove(_direction, Duration)
                .SetRelative(true);
        }
    }
}