using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes.Extensions;
using UnityEngine;

namespace PlayableNodes
{
    [Serializable]
    public class Track
    {
        [SerializeField] private bool _isActive = true;
        [SerializeField] private string _name;
        [SerializeField] private TrackNode[] _trackNodes;
        public bool IsActive => _isActive;
        public string Name => _name;
        public IReadOnlyList<TrackNode> Nodes => _trackNodes;

        public async UniTask PlayAsync(CancellationToken cancellationToken = default)
        {
            await _trackNodes.PlayAsync(cancellationToken: cancellationToken);
        }
    }
}