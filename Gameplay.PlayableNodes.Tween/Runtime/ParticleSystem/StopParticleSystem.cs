using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Particle
{
    [Serializable]
    public class StopParticleSystem : TargetAnimation<ParticleSystem>
    {
        protected override UniTask Play(CancellationToken cancellationToken)
        {
            return Target.StopAsync(Duration, cancellationToken: cancellationToken);
        }
    }
}