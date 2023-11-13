using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    public abstract class TargetAnimation<T> : IAnimation where T : Object
    {
        [SerializeField] private float _duration = 0.33f;
        [SerializeField] private float _delay;

        [field:NonSerialized] public T Target { get; private set; }
        public float Delay => _delay;

        public float Duration
        {
            get => _duration;
            protected set => _duration = value;
        }
        public abstract UniTask PlayAsync();

        public void SetTarget(Object target)
        {
            if (target is T t)
                Target = t;
            else
                throw new Exception($"Mismatch type Target={target.GetType()} should be {typeof(T).Name}");
        }
    }
}