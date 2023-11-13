using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableNodes
{
    [Serializable]
    public class ColorGraphic : TweenAnimation<Graphic>
    {
        [SerializeField] private ToFromValue<Color> _from;
        [SerializeField] private ToFromValue<Color> _to;
        protected override Tweener GenerateTween() => Target
            .DOColor(_to, Duration)
            .ChangeValuesColor(_to, _from);

    }
}