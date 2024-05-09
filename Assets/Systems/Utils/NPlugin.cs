using UnityEngine;
using System.Runtime.InteropServices;

namespace Nfynt
{
    public class NPlugin
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void OpenURL(string url);
        [DllImport("__Internal")]
        public static extern void OpenURLInSameTab(string url);
#else
        public static void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
#endif
    }
}