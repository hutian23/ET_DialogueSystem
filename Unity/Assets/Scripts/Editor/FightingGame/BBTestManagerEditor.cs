using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CustomEditor(typeof (BBTestManager))]
    public class BBTestManagerEditor: OdinEditor
    {
        public override void OnInspectorGUI()
        {
            BBTestManager testManager = target as BBTestManager;
            if (testManager == null) return;
            testManager.currentOrder = EditorGUILayout.IntPopup("Select Behavior: ",
                testManager.currentOrder,
                testManager.dropdownDict.Keys.ToArray(),
                testManager.dropdownDict.Values.ToArray());

            if (EditorApplication.isPlaying && GUILayout.Button("测试"))
            {
                EventSystem.Instance.Invoke(new BBTestManagerCallback()
                {
                    instanceId = testManager.instanceId, 
                    order = testManager.currentOrder
                });
            }
        }
    }
}