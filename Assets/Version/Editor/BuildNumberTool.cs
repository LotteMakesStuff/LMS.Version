using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace LMS.Version
{
    public class BuildNumberTool : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            // Find the version asset and increment its version number
            var assets = AssetDatabase.FindAssets($"t:{typeof(Version).Name}");
            Version versionAsset = null;

            if (assets.Length == 0)
            {
                versionAsset = VersionSettingProvider.GenerateNewVersionAsset();
            }
            else if (assets.Length > 1)
            {
                throw new BuildFailedException(
                    $"More than one Version asset in the project. Please ensure only one exists.\n{string.Join("\n", assets.Select(s => AssetDatabase.GUIDToAssetPath(s)))}");
            }
            else if (assets.Length == 1)
            {
                versionAsset = AssetDatabase.LoadAssetAtPath<Version>(AssetDatabase.GUIDToAssetPath(assets[0]));
            }


            var oldVersion = versionAsset.GameVersion.ToString();
            if (versionAsset.IsAutoIncrementEnabled)
            {
                versionAsset.GameVersion.Build++;
            }

            versionAsset.Initialize();

            // Push the version number into Unitys version field. Some console platforms really care about this!
            // (for example, xbox games can fail cert if the version number isnt changed here)
            PlayerSettings.bundleVersion = versionAsset.GameVersion.ToString();

            // Call into Git
            if (versionAsset.IsUpdateGitHashEnabled)
            {
                string gitHash = "00000000";
                try
                {
                    gitHash = GetGitCommitHash();
                }
                catch (Exception e)
                {
                    Debug.LogError("Could not call into git: " + e);
                }

                versionAsset.GitHash = gitHash;
            }

            versionAsset.BuildTimestamp = DateTime.UtcNow.ToString("yyyy MMMM dd - HH:mm");

            // Save changes
            EditorUtility.SetDirty(versionAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"BuildNumberTool.OnPreprocessBuild: Auto incrementing build number number. {oldVersion} -> {versionAsset.GameVersion}");
        }

        private static string GetGitCommitHash()
        {
            // example of how to hardcode the git path, if its not in your system PATH.
            // #if UNITY_EDITOR_WIN
            //         const string gitPath = "C:\\Program Files\\Git\\bin\\git.exe";
            // #endif
            const string gitPath = "git";
            var gitInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = gitPath,
                UseShellExecute = false,
            };

            var gitProcess = new Process();
            gitInfo.Arguments = "rev-parse --short HEAD"; // magic command to get the current commit hash
            gitInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            gitProcess.StartInfo = gitInfo;
            gitProcess.Start();
            var stdout = gitProcess.StandardOutput.ReadToEnd();
            gitProcess.WaitForExit();
            gitProcess.Close();
            stdout = stdout.Trim();
            return stdout;
        }
    }
}
