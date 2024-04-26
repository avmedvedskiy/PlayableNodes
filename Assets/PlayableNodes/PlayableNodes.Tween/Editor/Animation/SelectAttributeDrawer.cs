using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Animations
{
    public static class SelectAttributeDrawer
    {
        private static readonly GUIContent NullLabel = new("Null");
        public static void DrawAnimationDropDown(Rect position, GUIContent label, SerializedProperty property, string[] values)
        {
            var propertyRect = EditorGUI.PrefixLabel(position, label);
            if (EditorGUI.DropdownButton(propertyRect,new GUIContent(GetStringNameOrNUll(property)), FocusType.Keyboard))
            {
                CreateDropdown(property, values);
            }
        }
        private static GUIContent GetStringNameOrNUll(SerializedProperty property)
        {
            if (string.IsNullOrEmpty(property.stringValue))
            {
                return NullLabel;
            }

            return new GUIContent(property.stringValue);
        }

        private static void CreateDropdown(SerializedProperty property, string[] values)
        {
            GenericMenu nodesMenu = new GenericMenu();
            nodesMenu.AddItem(NullLabel, false, x => { OnSelect(string.Empty, property); }, null);
            foreach (var value in values)
            {
                nodesMenu.AddItem(new GUIContent(value), value == property.stringValue, x => { OnSelect((string)x, property); }, value);
            }

            nodesMenu.ShowAsContext();
        }

        private static void OnSelect(string data, SerializedProperty property)
        {
            property.stringValue = data;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}