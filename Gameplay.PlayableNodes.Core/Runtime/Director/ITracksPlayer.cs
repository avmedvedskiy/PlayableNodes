using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace PlayableNodes
{
    public interface ITracksPlayer
    {
        IReadOnlyList<Track> Tracks { get; }
        bool IsPlaying { get; }
        UniTask PlayAsync(string trackName,CancellationToken cancellationToken = default);
        
    }
}