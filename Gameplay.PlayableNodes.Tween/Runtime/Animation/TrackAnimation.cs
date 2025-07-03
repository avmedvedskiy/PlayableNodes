using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [Serializable]
    [Description("Plays an animation track from a TrackPlayerCollection")]
    public class TrackAnimation : TargetAnimation<TrackPlayerCollection>
    {
        [SerializeField] private string _animationName;

        protected override UniTask Play(CancellationToken cancellationToken) =>
            Target.PlayAsync(_animationName, cancellationToken);
    }
}