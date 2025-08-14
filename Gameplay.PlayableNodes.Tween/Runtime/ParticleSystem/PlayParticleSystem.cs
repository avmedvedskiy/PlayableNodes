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
        protected override async UniTask Play(CancellationToken cancellationToken)
        {
            await Target.PlayAsync(Duration, cancellationToken: cancellationToken);
        }
    }
}
