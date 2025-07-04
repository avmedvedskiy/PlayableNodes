using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    [ExecuteAlways]
    public class TrackTargetInteract : BaseTargetInteract
    {
        [SerializeField] private TrackPlayerCollection _collection;
        [SerializeField] private string _animationName = "Interact";

        [ContextMenu(nameof(Interact))]
        public override void Interact()
        {
            base.Interact();
            if (_collection != null)
                _collection.PlayAsync(_animationName).Forget();
        }
    }
}

