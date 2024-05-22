using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    public interface IChangeEndValue<in T>
    {
        void ChangeEndValue(T value);
    }
    
    /// <summary>
    /// Base interface for SerializedReference in editor
    /// </summary>
    public interface IAnimation
    {
        int Pin { get; }
        bool Enable { get; }
        float Delay { get; }
        float Duration { get; }
        UniTask PlayAsync(CancellationToken cancellationToken = default);
        void SetTarget(Object target);
    }
}