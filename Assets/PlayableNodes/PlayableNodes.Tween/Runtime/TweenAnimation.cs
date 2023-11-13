using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    public abstract class TweenAnimation<T> : TargetAnimation<T> where T : Object
    {
        [SerializeField] private Easing _ease = Easing.Default;

        public override UniTask PlayAsync() => RunTween().AwaitForComplete();

        private Tweener RunTween() =>
            GenerateTween()
                .SetEase(_ease)
                .SetRecyclable(true)
                .PlayOrPreview();

        protected abstract Tweener GenerateTween();
    }
}