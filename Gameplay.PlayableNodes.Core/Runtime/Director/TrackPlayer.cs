using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes.Extensions;
using UnityEngine;

namespace PlayableNodes
{
    public class TrackPlayer : MonoBehaviour, ITracksPlayer
    {
        [SerializeField] private Track _track;
        public IReadOnlyList<Track> Tracks => new[] { _track };

        public UniTask PlayAsync(string trackName,CancellationToken cancellationToken = default)
        {
            return _track.PlayAsync(cancellationToken);
        }
        
        private void OnDrawGizmosSelected()
        {
            this.DrawAnimationGizmos();
        }
    }
}