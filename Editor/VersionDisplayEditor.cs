using UnityEditor;
using UnityEngine;

namespace LMS.Version
{
    [CustomEditor(typeof(VersionDisplayBase), true)]
    public class VersionDisplayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUIStyle style = GUI.skin.GetStyle("HelpBox");
            style.richText = true;

            var guiColor = GUI.contentColor;
            GUI.enabled = false;
            GUI.contentColor = guiColor;
            EditorGUILayout.TextArea("Example format string\ngame <b>v{v}</b>\n<b>{gitHash}</b>", style);
            EditorGUILayout.TextArea("Output\ngame <b>v1.2.10</b>\n<b>12ab34cd</b>", style);
            EditorGUILayout.TextArea("Format codes\n" +
                                     "<b>{v}</b> Main game version \n" +
                                     "<b>{v_}</b> Main game version but with underscores between each number \n" +
                                     "<b>{gitHash}</b> short git commit hash. This is updated from git each time the project is built \n" +
                                     "<b>{time}</b> Build timestamp, this is updated each time the project is built\n" +
                                     "<b>{v:ExtraVersionName}</b> prints a version number taken from the extra versions array.",
                style);
            GUI.enabled = true;
            base.OnInspectorGUI();
        }
    }
}
