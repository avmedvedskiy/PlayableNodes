using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Particle
{
    [Serializable]
    [Description("Plays a ParticleSystem for the specified duration")]
    public class PlayParticleSystem : TargetAnimation<ParticleSystem>
    {
        protected override UniTask Play(CancellationToken cancellationToken)
        {
            return Target.PlayAsync(Duration, cancellationToken: cancellationToken);
        }
    }
}
