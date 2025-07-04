using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Moves the Transform from a start position to an end position")]
    public class MoveTransform : TweenAnimation<Transform>
    {
        [SerializeField] private MoveSpace _moveSpace;
        [SerializeField] private ToFromValue<Vector3> _from;
        [SerializeField] private ToFromValue<Vector3> _to;

        protected override Tween GenerateTween() =>
            Target
                .DOMove(_moveSpace, _to, Duration)
                .ChangeValuesVector(_to, _from);
    }
}