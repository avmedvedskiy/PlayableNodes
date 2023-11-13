using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace PlayableNodes
{
    /// <summary>
    /// Base interface for SerializedReference in editor
    /// </summary>
    public interface IAnimation
    {
        float Delay { get; }
        float Duration { get; }
        UniTask PlayAsync();
        void SetTarget(Object target);
    }
}