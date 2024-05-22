using System;
using DG.Tweening;
using PlayableNodes.Values;
using TMPro;
using UnityEngine;

namespace PlayableNodes.Text
{
    [Serializable]
    public class SizeByTextTransform : TweenAnimation<RectTransform>
    {
        [SerializeField] private ToFromValue<Vector2> _from;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _padding;
        [SerializeField] private Vector2 _clampSize = new(int.MinValue, int.MaxValue);

        protected override Tweener GenerateTween()
        {
            var rectSize = _text.rectTransform.sizeDelta;
            var to = _text.GetPreferredValues(_text.text, rectSize.x, rectSize.y);

            to.y = Mathf.Clamp(to.y + _padding, _clampSize.x, _clampSize.y);
            return Target
                .DOSizeDelta(to, Duration)
                .SetOptions(AxisConstraint.Y)
                .ChangeValuesVector(to, _from);
            
        }
        
    }
}