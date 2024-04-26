using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    public abstract class TargetAnimation<T> : IAnimation where T : Object
    {
        [SerializeField, HideInInspector] private bool _enable = true;
        [SerializeField] private float _duration = 0.33f;
        [SerializeField] private float _delay;

        [field: NonSerialized] public T Target { get; private set; }
        public bool Enable => _enable;
        public float Delay => _delay;

        public float Duration
        {
            get => _duration;
            protected set => _duration = value;
        }

        public UniTask PlayAsync(CancellationToken cancellationToken = default) =>
            Delay > 0
                ? UniTask.WaitForSeconds(Delay, cancellationToken: cancellationToken).ContinueWith(Play)
                : Play();

        protected abstract UniTask Play();

        public void SetTarget(Object target)
        {
            if (target is T t)
                Target = t;
            else
                throw new Exception($"Mismatch type Target={target.GetType()} should be {typeof(T).Name}");
        }
    }
}