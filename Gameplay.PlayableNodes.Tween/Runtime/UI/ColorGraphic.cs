using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableNodes
{
    [Serializable]
    [Description("Tweens the color of a UI Graphic component")]
    public class ColorGraphic : TweenAnimation<Graphic>
    {
        [SerializeField] private ToFromValue<Color> _from;
        [SerializeField] private ToFromValue<Color> _to;
        protected override Tween GenerateTween() => Target
            .DOColor(_to, Duration)
            .ChangeValuesColor(_to, _from);

    }
}