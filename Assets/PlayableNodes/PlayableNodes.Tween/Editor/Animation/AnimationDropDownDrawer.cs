using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Animations
{
    [CustomPropertyDrawer(typeof(AnimationNameSelectAttribute))]
    public class AnimationNameSelectAttributeDrawer : PropertyDrawer
    {
        private static readonly GUIContent NullLabel = new("Null");
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target = (Component)property.serializedObject.targetObject;
            var animation = target.GetComponentInChildren<Animation>() ?? target;
            var propertyRect = EditorGUI.PrefixLabel(position, label);
            if (EditorGUI.DropdownButton(propertyRect, new GUIContent(GetStringNameOrNUll(property)), FocusType.Keyboard))
            {
                CreateDropdown(property, animation.gameObject);
            }
        }
        
        public static void DrawSelectAnimationDropDown(
            Rect position, 
            SerializedProperty stringProperty,
            GameObject animationGameObject)
        {
            if (EditorGUI.DropdownButton(position, new GUIContent(GetStringNameOrNUll(stringProperty)), FocusType.Keyboard))
            {
                CreateDropdown(stringProperty, animationGameObject);
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

        private static void CreateDropdown(SerializedProperty property, GameObject target)
        {
            var animations = AnimationUtility.GetAnimationClips(target);
            GenericMenu nodesMenu = new GenericMenu();
            nodesMenu.AddItem(NullLabel, false, x => { OnSelect(string.Empty, property); }, null);
            if (animations != null)
                foreach (var animation in animations)
                {
                    string name = animation.name;
                    nodesMenu.AddItem(new GUIContent(name), name == property.stringValue, x => { OnSelect((string)x, property); }, name);
                }

            nodesMenu.ShowAsContext();
        }

        private static void OnSelect(string data, SerializedProperty property)
        {
            property.stringValue = data;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
    
    public static class AnimationDropDownDrawer
    {
    }
}