using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayableNodes
{
    public abstract class BaseTrackPlayer : MonoBehaviour
    {
        public abstract UniTask PlayAsync(string trackName);

        public void Play(string trackName) => PlayAsync(trackName).Forget();

        public abstract float TotalDuration(string trackName);
    }
}