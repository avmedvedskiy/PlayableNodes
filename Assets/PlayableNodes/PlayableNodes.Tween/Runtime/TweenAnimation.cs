using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    public abstract class TweenAnimation<T> : IAnimation where T : Object
    {
        [SerializeField, HideInInspector] private bool _enable = true;
        [SerializeField] private float _duration = 0.33f;
        [SerializeField] private float _delay;
        [SerializeField] private Easing _ease = Easing.Default;
        [field: NonSerialized] public T Target { get; private set; }
        public bool Enable => _enable;
        public float Delay => _delay;
        public float Duration
        {
            get => _duration;
            protected set => _duration = value;
        }

        public UniTask PlayAsync(CancellationToken cancellationToken = default) => RunTween().AwaitForComplete(cancellationToken: cancellationToken);

        private Tweener RunTween() =>
            GenerateTween()
                .SetDelay(Delay)
                .SetEase(_ease)
                .SetRecyclable(true)
                .PlayOrPreview();

        protected abstract Tweener GenerateTween();
        

        public void SetTarget(Object target)
        {
            if (target is T t)
                Target = t;
        }
    }
}