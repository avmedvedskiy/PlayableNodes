using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class EnableCollider : TargetAnimation<Collider>
    {
        [SerializeField] private bool _value;
        protected override UniTask Play(CancellationToken cancellationToken)
        {
            Target.enabled = _value;
            return UniTask.CompletedTask;
            
        }
    }
}