using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayableNodes.Extensions;
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
        public bool IsPlaying { get; private set; }

        public async UniTask PlayAsync(string trackName,CancellationToken cancellationToken = default)
        {
            Retarget();
            foreach (var track in _clip.Tracks)
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

        private void Retarget()
        {
            int index = 0;
            foreach (var track in _clip.Tracks)
            {
                foreach (var node in track.Nodes)
                {
                    node.SetContext(_bindings[index]);
                    index++;
                }
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            this.DrawAnimationGizmos();
        }
    }
}