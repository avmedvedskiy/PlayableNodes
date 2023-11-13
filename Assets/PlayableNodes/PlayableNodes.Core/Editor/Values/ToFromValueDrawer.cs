using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Values.Editor
{
    [CustomPropertyDrawer(typeof(ToFromValue<>),true)]
    public class ToFromValueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var typeProperty = property.FindPropertyRelative("_type");
            var valueProperty = property.FindPropertyRelative("_value");

            Rect typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width,
                EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(typeRect, typeProperty, GUIContent.none);

            if (typeProperty.intValue == (int)ToFromType.Direct)
            {
                EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
            }
            
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var easeProperty = property.FindPropertyRelative("_type");
            float height = easeProperty.intValue == (int)ToFromType.Direct ? 2f : 1f;
            return base.GetPropertyHeight(property, label) * height;
        }
    }
}