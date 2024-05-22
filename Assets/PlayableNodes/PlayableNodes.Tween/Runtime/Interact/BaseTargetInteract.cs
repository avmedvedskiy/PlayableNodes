using System;
using UnityEngine;

namespace PlayableNodes
{
    public interface ITargetInteract
    {
        event Action OnInteract;
        void Interact();
    }

    public abstract class BaseTargetInteract : MonoBehaviour, ITargetInteract
    {
        public event Action OnInteract;
        public virtual void Interact() => OnInteract?.Invoke();
    }
}