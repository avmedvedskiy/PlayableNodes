using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class AddForceRigidbody : TargetAnimation<Rigidbody>
    {
        [SerializeField] private Vector3 _force;
        [SerializeField] private ForceMode _mode = ForceMode.Impulse;
        protected override UniTask Play(CancellationToken cancellationToken)
        {
            Target.isKinematic = false;
            Target.AddForce(_force, _mode);
            return UniTask.WaitForSeconds(Duration, cancellationToken: cancellationToken);
        }
    }
}