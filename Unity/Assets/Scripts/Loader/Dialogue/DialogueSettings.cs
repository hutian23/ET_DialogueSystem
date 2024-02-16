using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueTree/DialogueTreeSettings", fileName = "DialogueTreeSettings")]
    public class DialogueSettings: ScriptableObject
    {
        [HideReferenceObjectPicker]
        [ShowInInspector, ReadOnly]
        public object copy;

        public Color DefaultColor;
        public Color PendingColor;
        public Color SuccessColor;
        public Color FailedColor;
        public Color ChoiceColor;

        public static DialogueSettings GetSettings()
        {
            return Resources.Load<DialogueSettings>("DialogueTreeSettings");
        }

        public string ExportPath => $"{Application.dataPath}/Config/Localization";

        [Space(10)]
        [HideInInspector]
        public string folderPath = "Assets/Res/ScriptableObject/DialogueTree";

        public DialogueTree GetTreeByID(uint treeID)
        {
            string[] guids = AssetDatabase.FindAssets("t:DialogueTree", new[] { folderPath });
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                DialogueTree tree = AssetDatabase.LoadAssetAtPath<DialogueTree>(assetPath);
                if (tree == null || tree.treeID != treeID) continue;
                return tree;
            }
            Debug.LogError($"不存在目标树: {treeID}");
            return null;
        }
    }
}