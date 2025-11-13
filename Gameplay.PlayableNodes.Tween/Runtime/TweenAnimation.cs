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
        [SerializeField, HideInInspector] private int _pin;
        [SerializeField] private float _duration = 0.33f;
        [SerializeField] private float _delay;
        [SerializeField] private Easing _ease = Easing.Default;
        [field: NonSerialized] public T Target { get; private set; }
        public int Pin => _pin;
        public bool Enable
        {
            get => _enable;
            set => _enable = value;
        }

        public float Delay => _delay;
        public float Duration
        {
            get => _duration;
            protected set => _duration = value;
        }

        public UniTask PlayAsync(CancellationToken cancellationToken = default) => 
            RunTween()
            .AwaitForComplete(TweenCancelBehaviour.CompleteWithSequenceCallbackAndCancelAwait, cancellationToken)
            .SuppressCancellationThrow();

        private Tween RunTween() =>
            GenerateTween()
                .SetDelay(Delay)
                .SetEase(_ease)
                .SetRecyclable(true)
                .PlayOrPreview();

        protected abstract Tween GenerateTween();
        

        public void SetTarget(Object target)
        {
            if (target is T t)
            {
                Target = t;
            }
            else if (typeof(T) == typeof(Transform) && target is Component component)
            {
                Target = component.transform as T;
            }
            else
            {
                throw new Exception($"Mismatch type Target={target.GetType()} should be {typeof(T).Name}");
            }
        }
    }
}