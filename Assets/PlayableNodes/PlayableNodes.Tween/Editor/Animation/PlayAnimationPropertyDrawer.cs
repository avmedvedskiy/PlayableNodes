using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [CustomPropertyDrawer(typeof(PlayAnimation))]
    public class PlayAnimationPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                Rect foldoutPosition =
                    new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
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
                    var target = (PlayAnimation)property.managedReferenceValue;


                    position.y += position.height;
                    SelectAttributeDrawer.DrawAnimationDropDown(
                        position,
                        new GUIContent(animationProperty.displayName), animationProperty,
                        GetAllAnimations(target.Target?.gameObject));

                    EditorGUI.indentLevel--;
                }
                
            }
        }

        private static string[] GetAllAnimations(GameObject target)
        {
            return target == null 
                ? new string[] { } 
                : AnimationUtility.GetAnimationClips(target).Select(x => x.name).ToArray();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }
    }
}