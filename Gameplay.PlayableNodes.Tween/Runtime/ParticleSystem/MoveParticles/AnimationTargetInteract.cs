using PlayableNodes.Animations;
using UnityEngine;

namespace PlayableNodes
{
    [ExecuteAlways]
    public class AnimationTargetInteract : BaseTargetInteract
    {
        [Tooltip("If Target == null, tween will be applied to this transfrom")]
        [SerializeField] private Animation _target;
        
        [SerializeField] private string _animationName = "Interact";
        
        [ContextMenu(nameof(Interact))]
        public override void Interact()
        {
            base.Interact();
            _target.PlayOrPreview(_animationName);
        }
    }
}