using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [Serializable]
    public class PlayAnimation : TargetAnimation<Animation>
    {
        [SerializeField,AnimationNameSelect] private string _animationName;

        public override UniTask PlayAsync() =>
            Target.PlayAsync(_animationName);
    }
}