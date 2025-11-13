using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Moves the Transform from a start position to an end position")]
    public class MoveTransform : TweenAnimation<Transform>, IChangeEndValue<Vector3>
    {
        [SerializeField] private MoveSpace _moveSpace;
        [SerializeField] private ToFromValue<Vector3> _from;
        [SerializeField] private ToFromValue<Vector3> _to;

        protected override Tween GenerateTween() =>
            Target
                .DOMove(_moveSpace, _to, Duration)
                .ChangeValuesVectorOnStart(_to, _from);

        public void ChangeEndValue(Vector3 value)
        {
            _to = new ToFromValue<Vector3>(value);
        }
    }
}