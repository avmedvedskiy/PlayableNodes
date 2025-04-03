using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes.Extensions;
using UnityEngine;

namespace PlayableNodes
{
    [HelpURL("https://github.com/avmedvedskiy/PlayableNodes")]
    public class TrackPlayer : MonoBehaviour, ITracksPlayer
    {
        [SerializeField] private Track _track;
        public IReadOnlyList<Track> Tracks => new[] { _track };
        public bool IsPlaying { get; private set; }

        public async UniTask PlayAsync(string trackName, CancellationToken cancellationToken = default)
        {
            IsPlaying = true;
            await _track.PlayAsync(cancellationToken);
            IsPlaying = false;
        }

        private void OnDrawGizmosSelected()
        {
            this.DrawAnimationGizmos();
        }
    }
}