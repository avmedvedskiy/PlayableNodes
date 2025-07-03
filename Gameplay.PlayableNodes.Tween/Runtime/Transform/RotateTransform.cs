using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Tweens the Transform's rotation to a target value")]
    public class RotateTransform : TweenAnimation<Transform>
    {
        [SerializeField] private MoveSpace _moveSpace;
        [SerializeField] private ToFromValue<Vector3> _to;

        protected override Tween GenerateTween()
        {
            return Target
                .DORotate(_moveSpace, _to, Duration);
                //.ChangeValuesVector(_to, _from);
        }
    }
}