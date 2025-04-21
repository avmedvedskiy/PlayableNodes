using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Values.Editor
{
    [CustomPropertyDrawer(typeof(ToFromValue<>),true)]
    public class ToFromValueDrawer : PropertyDrawer
    {
        private static readonly string[] Content = { "Direct", "Dynamic" };

        private SerializedProperty _typeProperty;
        private SerializedProperty _valueProperty;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            _typeProperty = property.FindPropertyRelative("_type");

            Rect typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width,
                EditorGUIUtility.singleLineHeight);

            //EditorGUI.PropertyField(typeRect, typeProperty, GUIContent.none);
            ShowDropDown(typeRect, _typeProperty);

            if (_typeProperty.intValue == (int)ToFromType.Direct)
            {
                _valueProperty = property.FindPropertyRelative("_value");
                EditorGUI.PropertyField(valueRect, _valueProperty, GUIContent.none);
            }
            
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private void ShowDropDown(Rect position, SerializedProperty property)
        {
            if (GUI.Button(
                    position,
                    Content[property.intValue],
                    EditorStyles.popup))
            {
                var menu = new GenericMenu();
                for (int i = 0; i < Content.Length; i++)
                {
                    int index = i;
                    menu.AddItem(new GUIContent(Content[i]), property.intValue == i, () =>
                    {
                        property.intValue = index;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _typeProperty = property.FindPropertyRelative("_type");
            float height = _typeProperty.intValue == (int)ToFromType.Direct ? 2f : 1f;
            return base.GetPropertyHeight(property, label) * height;
        }
    }
}