using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace PlayableNodes
{
    public interface ITracksPlayer
    {
        IReadOnlyList<Track> Tracks { get; }
        UniTask PlayAsync(string trackName);
        float TotalDuration(string trackName);
    }
}