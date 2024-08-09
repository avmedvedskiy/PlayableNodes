using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [CustomPropertyDrawer(typeof(TrackAnimation))]
    public class TrackAnimationPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position,label,property))
            {
                Rect foldoutPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, label, true);

                if (property.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    using (new DisableScope(false))
                    {
                        position.height = EditorGUIUtility.singleLineHeight;
                        position.y += position.height;
                        EditorGUI.PropertyField(position,property.FindPropertyRelative("_duration"));
                    }
                    
                    position.y += position.height;
                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_delay"));
                    var animationProperty = property.FindPropertyRelative("_animationName");
                    var target = (TrackAnimation)property.managedReferenceValue;

                    position.y += position.height;
                    SelectAttributeDrawer.DrawAnimationDropDown(
                        position,
                        new GUIContent(animationProperty.displayName), 
                        animationProperty, target.Target.Tracks.Select(x=> x.Name).ToArray());

                    EditorGUI.indentLevel--;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }
    }
}