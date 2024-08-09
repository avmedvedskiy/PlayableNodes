using UnityEngine;

namespace PlayableNodes
{
    public static class GUIExtensions
    {
        public enum Format
        {
            RichText,
            WordWrap,
            NoRichText,
            NoWordWrap,
        }
        
        /// <summary>Sets the bottom padding of the style</summary>
        public static GUIStyle PaddingBottom(this GUIStyle style, int bottom)
        {
            style.padding.bottom = bottom;
            return style;
        }

        public static GUIStyle AddAlignment(this GUIStyle style, TextAnchor anchor)
        {
            style.alignment = anchor;
            return style;
        }
        
        public static GUIStyle AddFontSize(this GUIStyle style, int fontSize)
        {
            style.fontSize = fontSize;
            return style;
        }

        public static GUIStyle AddColor(this GUIStyle style, Color color)
        {
            style.onHover.textColor = color;
            style.hover.textColor = color;
            style.onFocused.textColor = color;
            style.focused.textColor = color;
            style.onActive.textColor = color;
            style.active.textColor = color;
            style.onNormal.textColor = color;
            style.normal.textColor = color;
            return style;
        }
    }
}