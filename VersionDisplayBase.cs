using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMS.Version
{
    public abstract class VersionDisplayBase : MonoBehaviour
    {
        [Multiline(4)]
        public string VersionFormatString = "game v{v}\n{gitHash}";
        
        public string GetFormattedString(string format)
        {
            if (Version.Instance == null)
            {
                return "No Version asset found\n" + format;
            }
            
            var outString = format.Replace("{v}", Version.GetGameVersion(VersionDeliniator.Dot));
            outString = outString.Replace("{v_}", Version.GetGameVersion(VersionDeliniator.Underscore));
            outString = outString.Replace("{gitHash}", Version.Instance.GitHash);
            outString = outString.Replace("{time}", Version.Instance.BuildTimestamp);

            // handle extra versions in the format of {v:versionName}
            bool hasExtraVersions = true;
            while (hasExtraVersions)
            {
                var start = outString.IndexOf("{v:", StringComparison.InvariantCulture);
                if (start != -1)
                {
                    var end = outString.IndexOf("}", StringComparison.InvariantCulture);

                    var versionKey = outString.Substring(start + 3, end - start - 3);
                    outString = outString.Replace(outString.Substring(start, end - start + 1),
                        Version.GetExtraVersion(versionKey, VersionDeliniator.Dot));
                }
                else
                {
                    hasExtraVersions = false;
                }
            }

            return outString;
        }
    }

}