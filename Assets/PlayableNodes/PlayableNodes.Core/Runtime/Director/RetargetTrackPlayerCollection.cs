using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayableNodes.Experimental
{
    /// <summary>
    /// Experimental only, not support direct link to scene objects in scene
    /// </summary>
    public class RetargetTrackPlayerCollection : MonoBehaviour, ITracksPlayer
    {
        [SerializeField] private TrackClip _clip;
        [SerializeField] private List<Object> _bindings;
        public IReadOnlyList<Track> Tracks => _clip.Tracks;

        public UniTask PlayAsync(string trackName)
        {
            Retarget();
            foreach (var track in _clip.Tracks)
            {
                if (track.IsActive && track.Name == trackName)
                {
                    return track.PlayAsync();
                }
            }
            return UniTask.CompletedTask;
        }

        private void Retarget()
        {
            int index = 0;
            foreach (var track in _clip.Tracks)
            {
                foreach (var node in track.Nodes)
                {
                    node.SetContext(_bindings[index]);
                }
                index++;
            }
        }

        public float TotalDuration(string trackName)
        {
            var track = _clip.FindTrack(trackName);
            return track?.TotalDuration() ?? 0f;
        }
    }
}