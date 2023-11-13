using System;
using Cysharp.Threading.Tasks;
using DG.DOTweenEditor;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayableNodes
{
    
    [Serializable]
    public class MoveConstraintTransform : TargetAnimation<Transform>
    {
        [SerializeField] private MoveSpace _moveSpace;
        [SerializeField] private ToFromValue<Vector3> _from;
        [SerializeField] private ToFromValue<Vector3> _to;

        [SerializeField] private Easing _x;
        [SerializeField] private Easing _y;
        [SerializeField] private Easing _z;

        protected Sequence GenerateGlobalPositionTween()
        {
            var sequence = DOTween.Sequence();
            var currentPosition = Target.position;
            var toPosition = _to.ConvertValue(currentPosition);
            var fromPosition = _from.ConvertValue(currentPosition);
            return sequence
                .Join(Target
                    .DOMoveX(toPosition.x, Duration)
                    .SetEase(_x)
                    .ChangeStartValue(fromPosition))
                .Join(Target
                    .DOMoveY(toPosition.y, Duration)
                    .SetEase(_y)
                    .ChangeStartValue(fromPosition))
                .Join(Target
                    .DOMoveZ(toPosition.z, Duration)
                    .SetEase(_z)
                    .ChangeStartValue(fromPosition));
        }

        private Sequence Play()
        {
            var s = GenerateGlobalPositionTween();
            DOTweenEditorPreview.PrepareTweenForPreview(s, false);
            return s.Play();
        }

        public override UniTask PlayAsync()
        {
            return Play().AwaitForComplete();
        }
    }
    
}