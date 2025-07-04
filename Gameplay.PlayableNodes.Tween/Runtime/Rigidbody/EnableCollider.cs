using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Enables or disables a Collider")]
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