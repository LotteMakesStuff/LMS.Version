using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LMS.Version
{
    public class Version : ScriptableObject
    {
        static Version instance;

        public VersionNumber GameVersion = new VersionNumber();
        [SerializeField]private ExtraVersionNumber[] extraVersions;

        public string GitHash;
        public string BuildTimestamp;

        [Space]
        [SerializeField]
        private bool enableAutoIncrement = true;
        [SerializeField]
        private bool enableGitHash = true;

#if UNITY_EDITOR
        public bool IsAutoIncrementEnabled => enableAutoIncrement;
        public bool IsUpdateGitHashEnabled => enableGitHash;
#endif

        void OnEnable()
        {
            if (instance != null) return;
            instance = this;

            Initialize();
        }

        public void Initialize()
        {
            GameVersion.Initialize();
            if (extraVersions != null)
            {
                foreach (var extraVersion in extraVersions)
                {
                    extraVersion.Version.Initialize();
                }
            }
            
            
            if (Application.isEditor)
            {
                var defaultStaceTraceSetting = Application.GetStackTraceLogType(LogType.Log);
                Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
                
                var logOutput = "--------------------------\n"
                                + $"-- {Application.productName}\n"
                                + $"--------------------------\n"
                                + $"-- Version: {GameVersion}\n"
                                + $"-- Commit: {GitHash}\n"
                                + $"-- Built: {BuildTimestamp}\n"
                                + "--------------------------";
                Debug.Log(logOutput);
                
#if !DISABLE_LOG_STACKTRACE
                Application.SetStackTraceLogType(LogType.Log, defaultStaceTraceSetting);
#endif
            }
        }

        void OnDisable()
        {
            instance = null;
        }

        public static string GetGameVersion(VersionDeliniator deliniator = VersionDeliniator.Dot)
        {
            if (instance == null)
            {
                return "no version asset found";
            }
            
            return instance.GameVersion.Version(deliniator);
        }

        public static string GetExtraVersion(string name, VersionDeliniator deliniator = VersionDeliniator.Dot)
        {
            if (instance == null)
            {
                return "no version asset found";
            }
            
            var v = GetExtraVersion(name);
            if (v != null)
            {
                return v.Version(deliniator);
            }

            return name + " is not a valid version";
        }

        public static VersionNumber GetExtraVersion(string name)
        {
            if (instance.extraVersions != null)
            {
                foreach (var version in instance.extraVersions)
                {
                    if (version.VersionName.ToLower() == name.ToLower()) return version.Version;
                }
            }

            return null;
        }

        public static Version Instance
        {
            get
            {
                if (!(instance is null)) return instance;
                return null;
            }
        }
    }

    [Serializable]
    public class VersionNumber
    {
        public int Major;
        public int Minor;
        public int Build;

        private Dictionary<VersionDeliniator, string> version;

        public string Version(VersionDeliniator deliniator = VersionDeliniator.Dot) => version[deliniator];

        public void Initialize()
        {
            version = new Dictionary<VersionDeliniator, string>()
            {
                {VersionDeliniator.Dot, $"{Major}.{Minor}.{Build}"},
                {VersionDeliniator.Underscore, $"{Major}_{Minor}_{Build}"}
            };
        }

        public override string ToString()
        {
            return version[VersionDeliniator.Dot];
        }
    }

    [Serializable]
    public class ExtraVersionNumber
    {
        public string VersionName;
        public VersionNumber Version;
    }

    public enum VersionDeliniator
    {
        Dot,
        Underscore
    }
}
