using DG.Tweening;
using UnityEngine;

namespace PlayableNodes
{
    [ExecuteAlways]
    public class PunchTargetPositionInteract : BaseTargetInteract
    {
        [SerializeField] private Vector3 _punch = new Vector3(0.2f,0.2f,0.2f);
        [SerializeField] private float _duration = 0.33f;
        [SerializeField] private Easing _easing = Easing.Default;

        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _elasticity = 1f;

        private Tweener _tw;

        [ContextMenu(nameof(Interact))]
        public override void Interact()
        {
            base.Interact();
            _tw ??= transform
                .DOPunchPosition(_punch, _duration, _vibrato, _elasticity)
                .SetEase(_easing)
                .SetAutoKill(false);
            _tw.RestartOrPreview();
        }

        private void OnDisable()
        {
            _tw?.Kill();
            _tw = null;
        }

        private void OnValidate()
        {
            _tw = null;
        }
    }
}