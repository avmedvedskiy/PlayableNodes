using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Animations
{
    public static class AnimationNameSelectAttributeDrawer
    {
        public static void DrawAnimationDropDown(Rect position, GUIContent label, SerializedProperty property, GameObject target)
        {
            var animations = GetAllAnimations(target);
            SelectAttributeDrawer.DrawAnimationDropDown(position,label,property,animations);
        }

        private static string[] GetAllAnimations(GameObject target)
        {
            return AnimationUtility.GetAnimationClips(target).Select(x => x.name).ToArray();
        }
    }
}