using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ManagedReference;
using PlayableNodes.Extensions;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    [Serializable]
    public class TrackNode
    {
        [SerializeField] private bool _isActive;
        [SerializeField] private Object _context;

        [SerializeReference, DynamicReference(propertyName = nameof(_context))]
        private IAnimation[] _animations;
        public bool IsActive => _isActive;

        public Object Context => _context;

        public async UniTask PlayAsync(CancellationToken cancellationToken = default)
        {
            if (_context == null)
                return;

            await _animations.PlayAsync(_context, cancellationToken: cancellationToken);
        }

        public void SetContext(Object context) => _context = context;

        public float TotalDuration() => _animations.TotalDuration();

        public IReadOnlyList<IAnimation> Animations => _animations;
    }
}