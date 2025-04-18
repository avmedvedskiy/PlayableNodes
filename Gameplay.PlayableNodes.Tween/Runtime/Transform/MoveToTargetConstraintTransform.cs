using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class MoveToTargetConstraintTransform : TargetAnimation<Transform>, IChangeEndValue<Transform>
    {
        [SerializeField] private ToFromValue<Vector3> _from = ToFromValue<Vector3>.Dynamic;
        [SerializeField] private Transform _to;

        [SerializeField] private Easing _x = Easing.Default;
        [SerializeField] private Easing _y = Easing.Default;
        [SerializeField] private Easing _z = Easing.Default;
        [SerializeField] private bool _interactTarget = true;

        private Tween RunTween() =>
            Target
                .DOMoveConstraint(_from.ConvertValue(Target.position), _to.position, _x, _y, _z, Duration)
                .DOInteractWhenComplete(_to,_interactTarget)
                .PlayOrPreview();
        
        protected override UniTask Play(CancellationToken cancellationToken) => 
            RunTween()
                .AwaitForComplete(TweenCancelBehaviour.CompleteWithSequenceCallbackAndCancelAwait, cancellationToken);


        public void ChangeEndValue(Transform value) => _to = value;
    }
}