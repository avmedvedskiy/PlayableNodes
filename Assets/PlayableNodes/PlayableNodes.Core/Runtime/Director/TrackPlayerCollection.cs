using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    public class TrackPlayerCollection : BaseTrackPlayer
    {
        [SerializeField] private List<Track> _tracks;
        public override UniTask PlayAsync(string trackName)
        {
            foreach (var track in _tracks)
            {
                if (track.IsActive && track.Name == trackName)
                {
                    return track.PlayAsync();
                }
            }
            return UniTask.CompletedTask;
        }
        
        public override float TotalDuration(string trackName)
        {
            var track = _tracks.Find(x => x.Name == trackName);
            return track?.TotalDuration() ?? 0f;
        }
    }
}