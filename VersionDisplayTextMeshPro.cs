#if TEXTMESH
using TMPro;
using UnityEngine;

namespace LMS.Version
{
    public class VersionDisplayTextMeshPro : VersionDisplayBase
    {
        public TMP_Text text;

        void Start()
        {
            text.text = GetFormattedString(VersionFormatString);
        }
    }
}
#endif