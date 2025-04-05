using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Editor
{
    [CustomPropertyDrawer(typeof(Easing))]
    public class EasingDrawer : PropertyDrawer
    {
        private SerializedProperty _easeProperty;
        private SerializedProperty _curveProperty;
        private SerializedProperty _scaleProperty;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            _easeProperty ??= property.FindPropertyRelative("_ease");
            _curveProperty ??= property.FindPropertyRelative("_curve");
            _scaleProperty ??= property.FindPropertyRelative("_scale");

            float offset = 5f;
            float halfWidth = position.width / 2f;
            Rect easeRect = new Rect(position.x, position.y, halfWidth, EditorGUIUtility.singleLineHeight);
            Rect curveRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width,
                EditorGUIUtility.singleLineHeight);
            Rect scaleRect = new Rect(position.x + halfWidth + offset, position.y, halfWidth - offset,
                EditorGUIUtility.singleLineHeight);

            //EditorGUI.PropertyField(easeRect, easeProperty, GUIContent.none);
            if (EditorGUI.DropdownButton(easeRect, new GUIContent(((Ease)_easeProperty.intValue).ToString()), FocusType.Keyboard))
            {
                var window = new EaseSelectionWindow();
                window.Prepare((Ease)_easeProperty.intValue, x =>
                {
                    _easeProperty.intValue = (int)x;
                    _easeProperty.serializedObject.ApplyModifiedProperties();
                });
                PopupWindow.Show(easeRect, window);
            }

            if (_easeProperty.intValue == (int)Ease.INTERNAL_Custom)
            {
                EditorGUI.PropertyField(curveRect, _curveProperty, GUIContent.none);
            }

            EditorGUI.PropertyField(scaleRect, _scaleProperty, GUIContent.none);


            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _easeProperty ??= property.FindPropertyRelative("_ease");
            float height = _easeProperty.intValue == (int)Ease.INTERNAL_Custom ? 2f : 1f;
            return base.GetPropertyHeight(property, label) * height;
        }
    }
}