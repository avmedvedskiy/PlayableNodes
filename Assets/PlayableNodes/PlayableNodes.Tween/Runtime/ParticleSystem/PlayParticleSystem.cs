using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Particle
{
    [Serializable]
    public class PlayParticleSystem : TargetAnimation<ParticleSystem>
    {
        protected override UniTask Play()
        {
            return Target.PlayAsync(Duration);
        }
    }
}
