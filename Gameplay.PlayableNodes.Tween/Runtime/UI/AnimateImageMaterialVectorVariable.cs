﻿using System;
using DG.Tweening;
using PlayableNodes.Animations;
using PlayableNodes.Values;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    public class AnimateImageMaterialVectorVariable : TweenAnimation<Image>
    {
        [SerializeField] private Material _material;
        [SerializeField] private Vector4 _from;
        [SerializeField] private Vector4 _to;
        [SerializeField] private string _fieldName;
        [SerializeField] private bool _resetMaterialOnComplete;

        private Vector4 _current;
        private Material _lastMaterial;
        private Material _currentMaterial;

        protected override Tween GenerateTween()
        {
            _current = _from;
            return DOTween
                .To(() => _current, Set, _to, Duration)
                .OnStart(OnStart)
                .OnComplete(OnComplete)
                .ChangeStartValue(_current);
        }

        private void OnComplete()
        {
            if (Target && _resetMaterialOnComplete)
            {
                Target.material.DestroyOrPreview();
                Target.material = _lastMaterial;
            }
        }

        private void OnStart()
        {
            _current = _from;
            _lastMaterial = Target.material;
            Target.material = Object.Instantiate(_material);
        }

        private void Set(Vector4 value)
        {
            _current = value;
            Target.materialForRendering.SetVector(_fieldName, value);
        }
    }
}