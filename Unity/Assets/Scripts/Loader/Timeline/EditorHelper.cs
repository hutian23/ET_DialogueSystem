using System.IO;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    public static class EditorHelper
    {
#if UNITY_EDITOR
        public static string GetPath(this TextAsset asset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            return Path.GetFullPath(assetPath);
        }
#endif
    }
}