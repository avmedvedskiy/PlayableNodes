using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    [Description("Tweens the color of a SpriteRenderer component")]
    public class SpriteColorGraphic : TweenAnimation<SpriteRenderer>
    {
        [SerializeField] private ToFromValue<Color> _from = new(Color.white);
        [SerializeField] private ToFromValue<Color> _to = new(Color.white) ;
        protected override Tween GenerateTween() => Target
            .DOColor(_to, Duration)
            .ChangeValuesColor(_to, _from);

    }
}