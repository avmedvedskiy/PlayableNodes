using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Invokes Interact on the target Transform if available")]
    public class InteractTarget : TargetAnimation<Transform>, IChangeEndValue<Transform>
    {
        [SerializeField] private Transform _to;
        protected override UniTask Play(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                if(_to.TryGetComponent<ITargetInteract>(out var interact))
                    interact.Interact();
            }
            
            return UniTask.CompletedTask;
        }

        public void ChangeEndValue(Transform value) => _to = value;
    }
}