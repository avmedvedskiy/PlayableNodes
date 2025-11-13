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
        [SerializeField, HideInInspector] private int _pin;
        [SerializeField] private float _duration = 0.33f;
        [SerializeField] private float _delay;

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

        public async UniTask PlayAsync(CancellationToken cancellationToken = default)
        {
            if (Delay > 0)
            {
                await UniTask
                    .WaitForSeconds(Delay, cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
                
                if(cancellationToken.IsCancellationRequested)
                    return;
            }
            await Play(cancellationToken);
            
        }

        protected abstract UniTask Play(CancellationToken cancellationToken);

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