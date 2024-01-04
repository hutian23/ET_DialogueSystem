using System.Text.RegularExpressions;
using ET.Client;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class DialogueTest : MonoBehaviour
    {
        [TextArea]
        public string desc;
        
        [ContextMenu("dialogueTest")]
        public void Test()
        {
            string pattern = @"<model(\s+type=\w+)?(\s+name=\w+)?\s*/?>";
            Regex regex = new(pattern);
            
            MatchCollection collections = regex.Matches(this.desc);
            foreach (var m in collections)
            {
                string mtp = @"<model(?:\s+type=\w+\s+name=)(\w+)";
                Regex mtp_re = new(mtp);
                MatchCollection matches = regex.Matches(m.ToString());
                foreach (var n in matches)
                {
                    Debug.Log(n);
                }
            }
        }
        
        [ContextMenu("Tools/Load DialogueTree SO Files")]
        private void LoadDialogueTreeSOFiles()
        {
            string folderPath = "Assets/Res/ScriptableObject/DialogueTree";
            string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });
            foreach (var guid in assetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof (DialogueTree));
                if (asset != null && asset is DialogueTree tree)
                {
                    Debug.Log(tree.treeName);
                }
            }
            // string path = "Assets/Res/ScriptableObject/DialogueTree/";
            // UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            // Debug.Log(assets.Length);
            // foreach (UnityEngine.Object asset in assets)
            // {
            //     if (asset is DialogueTree)
            //     {
            //         DialogueTree dialogueTreeSO = (DialogueTree)asset;
            //         // 在这里执行你想要的操作，比如打印名称
            //         Debug.Log("Loaded Dialogue Tree SO: " + dialogueTreeSO.treeName);
            //     }
            // }
        }
    }
}
