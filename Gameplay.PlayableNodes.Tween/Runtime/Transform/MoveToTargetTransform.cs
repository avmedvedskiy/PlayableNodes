using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Moves a Transform to a target position with easing and dynamic start/end support")]
    public class MoveToTargetTransform : TargetAnimation<Transform>, IChangeEndValue<Transform>
    {
        [SerializeField] private ToFromValue<Vector3> _from = ToFromValue<Vector3>.Dynamic;
        [SerializeField] private Transform _to;

        [SerializeField] private Easing _ease = Easing.Default;

        private Tween RunTween() =>
            Target
                .DOMove(_to.position, Duration)
                .SetEase(_ease)
                .ChangeValues( _from.ConvertValue(Target.position),_to.position)
                .DOInteractWhenComplete(_to)
                .PlayOrPreview();
        
        protected override UniTask Play(CancellationToken cancellationToken) => 
            RunTween()
                .AwaitForComplete(TweenCancelBehaviour.CompleteWithSequenceCallbackAndCancelAwait, cancellationToken);


        public void ChangeEndValue(Transform value) => _to = value;
    }
}