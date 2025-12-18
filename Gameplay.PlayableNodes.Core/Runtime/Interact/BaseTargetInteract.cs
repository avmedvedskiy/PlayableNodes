using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayableNodes
{
    public interface ITargetInteract
    {
        event UnityAction OnInteract;
        void Interact();
    }

    public abstract class BaseTargetInteract : MonoBehaviour, ITargetInteract
    {
        [SerializeField] private UnityEvent _onInteract;
        public event UnityAction OnInteract
        {
            add => _onInteract.AddListener(value);
            remove => _onInteract.RemoveListener(value);
        }
        public virtual void Interact() => _onInteract?.Invoke();
    }
}