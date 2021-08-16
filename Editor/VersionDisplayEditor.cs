using System.Collections;
using System.Collections.Generic;
using LMS.Version;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VersionDisplayBase), true)]
public class VersionDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Example format string\ngame v{v}\n{gitHash}\n\ngame v1.2.10\n12ab34cd\n\n" +
                                "Format codes\n" +
                                "{v} Main game version \n" +
                                "{v_} Main game version but with underscores between each number \n" +
                                "{gitHash} short git commit hash. This is updated from git each time the project is built \n" +
                                "{time} Build timestamp, this is updated each time the project is built\n" +
                                "{v:ExtraVersionName} prints a version number taken from the extra versions array.\n", MessageType.Info);
        base.OnInspectorGUI();
    }
}
