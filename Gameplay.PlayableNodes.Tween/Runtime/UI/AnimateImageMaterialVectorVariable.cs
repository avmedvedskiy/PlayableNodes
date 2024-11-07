using System;
using DG.Tweening;
using PlayableNodes.Values;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableNodes
{
    [Serializable]
    public class AnimateImageMaterialVectorVariable : TweenAnimation<Image>
    {
        [SerializeField] private Material _material;
        [SerializeField] private Vector3 _from;
        [SerializeField] private Vector3 _to;
        [SerializeField] private string _fieldName;

        private Vector3 _current;
        private Material _lastMaterial;

        protected override Tween GenerateTween()
        {
            _current = _from;
            return DOTween
                .To(() => _current, SetVector, _to, Duration)
                .OnStart(OnStart)
                .OnComplete(OnComplete)
                .ChangeStartValue(_current);
        }

        private void OnComplete()
        {
            if (Target)
                Target.material = _lastMaterial;
        }

        private void OnStart()
        {
            _lastMaterial = Target.material;
            if(_material != null)
                Target.material = _material;
        }

        private void SetVector(Vector3 value)
        {
            _current = value;
            Target.material.SetVector(_fieldName, value);
        }
    }
}