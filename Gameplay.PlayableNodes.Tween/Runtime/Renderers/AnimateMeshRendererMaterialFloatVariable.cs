using System;
using System.ComponentModel;
using DG.Tweening;
using PlayableNodes.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    [Description("Tweens a float property on a MeshRenderer material and optionally restores the original material on completion")]
    public class AnimateMeshRendererMaterialFloatVariable : TweenAnimation<MeshRenderer>
    {
        [SerializeField] private Material _material;
        [SerializeField] private float _from;
        [SerializeField] private float _to;
        [SerializeField] private string _fieldName;
        [SerializeField] private bool _resetMaterialOnComplete = true;

        private Material _lastMaterial;

        protected override Tween GenerateTween()
        {
            return DOTween
                .To(() => _from, Set, _to, Duration)
                .OnStart(OnStart)
                .OnComplete(OnComplete)
                .ChangeStartValue(_from);
        }

        private void OnStart()
        {
          if (_material != null)
          {
            _lastMaterial = Target.sharedMaterial;
            Target.material = Object.Instantiate(_material);
          }
          Set(_curve.Evaluate(0f));
        }

        private void OnComplete()
        {
            if (Target && _resetMaterialOnComplete && _material != null)
            {
                Target.material.DestroyOrPreview();
                Target.material = _lastMaterial;
            }
        }

        private void Set(float value)
        {
            Target.material.SetFloat(_fieldName, value);
        }
    }
}
