using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [Serializable]
    public class TrackAnimation : TargetAnimation<TrackPlayerCollection>
    {
        [SerializeField] private string _animationName;

        protected override UniTask Play() =>
            Target.PlayAsync(_animationName);
    }
}