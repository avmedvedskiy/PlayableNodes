using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayableNodes
{
    [Serializable]
    public class AddRandomForceRigidbody : TargetAnimation<Rigidbody>
    {
        [SerializeField] private Vector3 _forceMin;
        [SerializeField] private Vector3 _forceMax;
        [SerializeField] private ForceMode _mode = ForceMode.Impulse;

        protected override UniTask Play(CancellationToken cancellationToken)
        {
            Target.isKinematic = false;
            Target.AddForce(new Vector3(
                Random.Range(_forceMin.x, _forceMax.x),
                Random.Range(_forceMin.y, _forceMax.y),
                Random.Range(_forceMin.z, _forceMax.z)
            ), _mode);
            return UniTask.WaitForSeconds(Duration, cancellationToken: cancellationToken);
        }
    }

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