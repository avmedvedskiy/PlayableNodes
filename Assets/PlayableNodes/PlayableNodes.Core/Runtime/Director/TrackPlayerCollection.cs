using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    public class TrackPlayerCollection : MonoBehaviour, ITracksPlayer
    {
        [SerializeField] private List<Track> _tracks;
        public IReadOnlyList<Track> Tracks => _tracks;

        public UniTask PlayAsync(string trackName,CancellationToken cancellationToken = default)
        {
            foreach (var track in _tracks)
            {
                if (track.IsActive && track.Name == trackName)
                {
                    return track.PlayAsync(cancellationToken);
                }
            }
            return UniTask.CompletedTask;
        }
        
        public float TotalDuration(string trackName)
        {
            var track = _tracks.Find(x => x.Name == trackName);
            return track?.TotalDuration() ?? 0f;
        }
    }
}