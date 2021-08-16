using UnityEngine;
using UnityEngine.UI;

namespace LMS.Version
{
    public class VersionDisplay : VersionDisplayBase
    {
        public Text text;
        
        void Start()
        {
            text.text = GetFormattedString(VersionFormatString);
        }
    }
}
