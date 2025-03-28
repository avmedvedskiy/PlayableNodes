using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes.Extensions;
using UnityEngine;

namespace PlayableNodes
{
    public class TrackPlayerCollection : MonoBehaviour, ITracksPlayer
    {
        [SerializeField] private List<Track> _tracks;
        public IReadOnlyList<Track> Tracks => _tracks;
        public bool IsPlaying { get; private set; }

        public async UniTask PlayAsync(string trackName,CancellationToken cancellationToken = default)
        {
            foreach (var track in _tracks)
            {
                if (track.IsActive && track.Name == trackName)
                {
                    IsPlaying = true;
                    await track.PlayAsync(cancellationToken);
                    IsPlaying = false;
                }
            }
            Debug.LogWarning($"Not found track name {trackName}");
        }

        private void OnDrawGizmosSelected()
        {
            this.DrawAnimationGizmos();
        }
    }
}