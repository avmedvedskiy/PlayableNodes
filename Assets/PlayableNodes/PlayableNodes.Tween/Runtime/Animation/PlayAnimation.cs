using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [Serializable]
    public class PlayAnimation : TargetAnimation<Animation>
    {
        [SerializeField] private string _animationName;

        protected override UniTask Play(CancellationToken cancellationToken) =>
            Target.PlayAsync(_animationName,cancellationToken);
    }
}