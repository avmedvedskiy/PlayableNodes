using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class MoveToInteractTargetConstraintTransform : TargetAnimation<Transform>
    {
        [SerializeField] private ToFromValue<Vector3> _from;
        [SerializeField] private BaseTargetInteract _to;

        [SerializeField] private Easing _x = Easing.Default;
        [SerializeField] private Easing _y = Easing.Default;
        [SerializeField] private Easing _z = Easing.Default;

        private Tween RunTween() =>
            Target
                .DOMoveConstraint(_from.ConvertValue(Target.position), _to.transform.position, _x, _y, _z, Duration)
                //.SetDelay(Delay)
                .SetRecyclable(true)
                .OnComplete(_to.Interact)
                .PlayOrPreview();
        
        protected override UniTask Play() => RunTween().AwaitForComplete();
    }
}