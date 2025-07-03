using System;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [Serializable]
    [Description("Plays an Animation clip by name")]
    public class PlayAnimation : TargetAnimation<Animation>
    {
        [SerializeField] private string _animationName;

        protected override UniTask Play(CancellationToken cancellationToken) =>
            Target.PlayOrPreviewAsync(_animationName,cancellationToken);
    }
}