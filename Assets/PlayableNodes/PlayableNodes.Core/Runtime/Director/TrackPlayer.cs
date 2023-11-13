using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    public class TrackPlayer : BaseTrackPlayer
    {
        [SerializeField] private Track _track;
        public override UniTask PlayAsync(string trackName)
        {
            return _track.PlayAsync();
        }

        public override float TotalDuration(string trackName) => _track.TotalDuration();
    }
}