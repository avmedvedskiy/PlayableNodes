using System;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Moves the Transform by a relative offset in the specified space")]
    public class MoveRelativeTransform : TweenAnimation<Transform>
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