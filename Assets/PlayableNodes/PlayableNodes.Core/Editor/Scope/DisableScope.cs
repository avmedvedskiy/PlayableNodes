using UnityEngine;

namespace PlayableNodes
{
    public class DisableScope : EditorGUIScope
    {
        public DisableScope(bool enabled)
        {
            GUI.enabled = enabled;
        }
        
        protected override void CloseScope()
        {
            GUI.enabled = true;
        }
    }
}