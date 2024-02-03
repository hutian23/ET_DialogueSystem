using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
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

        [Space(10)]
        public List<DialogueTree> exportTrees;
        public string ExportPath => $"{Application.dataPath}/Config/Localization";

        [Button("导出对话树")]
        public void Exports()
        {
            if (Directory.Exists(ExportPath)) Directory.Delete(ExportPath, true);
            Directory.CreateDirectory(ExportPath); 
            exportTrees.ForEach(tree => { tree.Export(); });
        }
    }
}