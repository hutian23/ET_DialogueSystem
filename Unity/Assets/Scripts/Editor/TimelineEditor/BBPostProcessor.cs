using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Timeline.Editor
{
    public class BBPostProcessor : AssetPostprocessor
    {
        // 资源导入之后的处理
        static void OnPostprocessAllAssets(
        string[] importAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
        {
            foreach (string str in importAssets)
            {
                // Debug.Log("BBPostProcessor: " + str);

                var bb_obj = AssetDatabase.LoadAssetAtPath<Object>(str);
                // AssetDatabase.SetLabels(bb_obj, new []{"bb"});
            }

            // foreach (string str in deletedAssets)
            // {
            //     Debug.Log("Deleted Asset: " + str);
            // }
            //
            // for (int i = 0; i < movedAssets.Length; i++)
            // {
            //     Debug.Log("Moved Asset: " + movedAssets[i] + "from: " + movedFromAssetPaths[i]);
            // }
        }

        // private void OnPreprocessAsset()
        // {
        //     // if (assetImporter.assetPath.EndsWith(".bb"))
        //     // {
        //     //     Debug.Log("BBPreProcessor: " + assetImporter.assetPath);
        //     // }
        // }
    }
}