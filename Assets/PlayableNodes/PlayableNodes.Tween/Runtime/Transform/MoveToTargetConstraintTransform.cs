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
        [SerializeField] private ToFromValue<Vector3> _from;
        [SerializeField] private Transform _to;

        [SerializeField] private Easing _x = Easing.Default;
        [SerializeField] private Easing _y = Easing.Default;
        [SerializeField] private Easing _z = Easing.Default;

        private Tween RunTween() =>
            Target
                .DOMoveConstraint(_from.ConvertValue(Target.position), _to.position, _x, _y, _z, Duration)
                .SetRecyclable(true)
                .OnComplete(OnInteract)
                .PlayOrPreview();
        
        protected override UniTask Play(CancellationToken cancellationToken) => 
            RunTween()
                .AwaitForComplete(TweenCancelBehaviour.CompleteWithSequenceCallbackAndCancelAwait, cancellationToken);

        private void OnInteract()
        {
            if(_to.TryGetComponent<ITargetInteract>(out var interact))
                interact.Interact();
        }

        public void ChangeEndValue(Transform value) => _to = value;
    }
}