using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Particle
{
    [Serializable]
    [Description("Plays particles moving towards a target Transform")]
    public class PlayParticleToTarget : TargetAnimation<MoveParticlesToTarget>, IChangeEndValue<Transform>
    {
        [SerializeField] private Transform _to;
        protected override UniTask Play(CancellationToken cancellationToken)
        {
            Target.SetTarget(_to);
            return Target.System.PlayAsync(Duration, cancellationToken: cancellationToken);
        }

        public void ChangeEndValue(Transform value) => _to = value;
    }
}