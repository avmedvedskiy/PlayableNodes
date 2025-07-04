using System;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
[Serializable]
[Description("Orients a Transform to face a target Transform")]
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