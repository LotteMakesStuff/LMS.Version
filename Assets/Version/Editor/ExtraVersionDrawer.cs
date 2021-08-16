using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LMS.Version
{
    [CustomPropertyDrawer(typeof(ExtraVersionNumber))]
    public class ExtraVersionDrawer : PropertyDrawer
    {
        
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var name = property.FindPropertyRelative("VersionName");
            var version  = property.FindPropertyRelative("Version");
            
            var major = version.FindPropertyRelative("Major");
            var minor = version.FindPropertyRelative("Minor");
            var build = version.FindPropertyRelative("Build");

            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, name);
            position.y += EditorGUIUtility.singleLineHeight;
            var nums = new[] { major.intValue, minor.intValue, build.intValue };
            EditorGUI.MultiIntField(position, new []{new GUIContent("Major"), new GUIContent("Minor"), new GUIContent("Build"), }, nums);

            major.intValue = nums[0];
            minor.intValue = nums[1];
            build.intValue = nums[2];
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label)*2;
        }
    }
}