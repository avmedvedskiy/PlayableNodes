using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class MoveTransform : TweenAnimation<Transform>
    {
        [SerializeField] private MoveSpace _moveSpace;
        [SerializeField] private ToFromValue<Vector3> _from;
        [SerializeField] private ToFromValue<Vector3> _to;

        protected override Tweener GenerateTween() =>
            Target
                .DOMove(_moveSpace, _to, Duration)
                .ChangeValuesVector(_to, _from);
    }
}