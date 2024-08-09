using UnityEditor;
using UnityEngine;

namespace PlayableNodes
{
    public static class DrawerTools
    {


        public static void ColoredToggleProperty(SerializedProperty boolProperty, params GUILayoutOption[] options)
        {
            if (boolProperty != null)
            {
                using (new ColorScope(boolProperty.boolValue ? Color.green : Color.red))
                {
                    boolProperty.boolValue = EditorGUILayout.Toggle(boolProperty.boolValue, options);
                }
            }
        }

        public static void ColoredToggleLeft(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property != null)
            {
                using (new ColorScope(property.boolValue ? Color.green : Color.red))
                {
                    property.boolValue =
                        EditorGUI.ToggleLeft(position, label,
                            property.boolValue);
                }
            }
        }
    }
}