using UnityEngine;

namespace PlayableNodes
{
    public class ColorScope : EditorGUIScope
    {
        private readonly Color _prevBackground;
        private readonly Color _prevContent;
        private readonly Color _prevMain;

        public ColorScope(Color? background, Color? content = null, Color? main = null)
        {
            this._prevBackground = GUI.backgroundColor;
            this._prevContent = GUI.contentColor;
            this._prevMain = GUI.color;
            if (background.HasValue)
                GUI.backgroundColor = background.Value;
            if (content.HasValue)
                GUI.contentColor = content.Value;
            if (!main.HasValue)
                return;
            GUI.color = main.Value;
        }

        protected override void CloseScope()
        {
            GUI.backgroundColor = this._prevBackground;
            GUI.contentColor = this._prevContent;
            GUI.color = this._prevMain;
        }
    }
}