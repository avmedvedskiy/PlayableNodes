using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class RotateTransform : TweenAnimation<Transform>
    {
        [SerializeField] private MoveSpace _moveSpace;
        [SerializeField] private ToFromValue<Vector3> _from;
        [SerializeField] private ToFromValue<Vector3> _to;

        protected override Tweener GenerateTween()
        {
            return Target
                .DORotate(_moveSpace, _to, Duration);
                //.ChangeValuesVector(_to, _from);
        }
    }
}