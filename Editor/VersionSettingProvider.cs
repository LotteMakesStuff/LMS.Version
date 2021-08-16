using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LMS.Version
{
     public class VersionSettingProvider : SettingsProvider
    {
        public VersionSettingProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public static bool IsSettingsAvailable()
        {
            return true;
        }

        private Version versionAsset;

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var assets = AssetDatabase.FindAssets($"t:{typeof(Version).Name}");

            if (assets == null || assets.Length == 0 )
            {
                return;
            }
            
            versionAsset = AssetDatabase.LoadAssetAtPath<Version>(AssetDatabase.GUIDToAssetPath(assets[0]));
        }

        public override void OnGUI(string searchContext)
        {
            if (versionAsset == null)
            {
                GUILayout.Label("Create Version Asset");
                if (GUILayout.Button("Do it..."))
                {
                    versionAsset = GenerateNewVersionAsset();
                }
                return;
            }
            
            // Use IMGUI to display UI:
            var serializedObject = new SerializedObject(versionAsset);
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {

                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);

                } while (prop.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }


        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new VersionSettingProvider("Project/Version", SettingsScope.Project);
                
                return provider;
            }

            return null;
        }
        
        internal static Version GenerateNewVersionAsset()
        {
            Version versionAsset;
            // Lets see if the unity project version string is parseable into a usable version number. if so we can auto generate one
            var split = PlayerSettings.bundleVersion.Split('.');

            int major = 1, minor = 0, build = 0;
            if (split.Length == 3)
            {
                int.TryParse(split[0], out major);
                int.TryParse(split[1], out minor);
                int.TryParse(split[2], out build);
            }

            // try create a new version asset.
            versionAsset = Version.CreateInstance<Version>();
            versionAsset.GameVersion.Major = major;
            versionAsset.GameVersion.Minor = minor;
            versionAsset.GameVersion.Build = build;
            versionAsset.Initialize();

            // save it out!
            AssetDatabase.CreateAsset(versionAsset, "Assets/Version.asset");
            AssetDatabase.SaveAssets();
            Debug.Log("Generating Version asset", versionAsset);

            // do some initial setup on the version asset
            // we want to try and make sure the first object that has an opportunity to write into the log is the version system.
            // this means that the games version information will be right at the top of any player.log we get sent by
            // players asking for support, right under where the core unity engine puts the system specification. THIS IS SO USEFUL!

            // hook the version asset into the preload asset list...
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.Add(versionAsset);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());

            // Set the execution order...
            MonoScript script = MonoScript.FromScriptableObject(versionAsset);
            MonoImporter.SetExecutionOrder(script, -32000);

            return versionAsset;
        }
    }
}