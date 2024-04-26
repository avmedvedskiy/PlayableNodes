using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    /// <summary>
    /// Base interface for SerializedReference in editor
    /// </summary>
    public interface IAnimation
    {
        bool Enable { get; }
        float Delay { get; }
        float Duration { get; }
        UniTask PlayAsync(CancellationToken cancellationToken = default);
        void SetTarget(Object target);
    }
}