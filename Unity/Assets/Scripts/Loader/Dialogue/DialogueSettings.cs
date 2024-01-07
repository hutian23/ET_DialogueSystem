using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    [CreateAssetMenu(menuName = "ScriptableObject/DialogueTree/DialogueTreeSettings", fileName = "DialogueTreeSettings")]
    public class DialogueSettings: ScriptableObject
    {
        [HideReferenceObjectPicker]
        [ShowInInspector, ReadOnly]
        public object copyNode;

        public Color DefaultColor;
        public Color PendingColor;
        public Color SuccessColor;
        public Color FailedColor;

        public static DialogueSettings GetSettings()
        {
            return Resources.Load<DialogueSettings>("DialogueTreeSettings");
        }
    }
}