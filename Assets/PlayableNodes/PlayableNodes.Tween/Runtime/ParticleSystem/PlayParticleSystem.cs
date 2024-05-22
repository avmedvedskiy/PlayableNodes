using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Particle
{
    [Serializable]
    public class PlayParticleSystem : TargetAnimation<ParticleSystem>
    {
        protected override UniTask Play(CancellationToken cancellationToken)
        {
            return Target.PlayAsync(Duration, cancellationToken: cancellationToken);
        }
    }
}
