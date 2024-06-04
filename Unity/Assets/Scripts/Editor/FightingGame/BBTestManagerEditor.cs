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

            if (GUILayout.Button("Test Behavior"))
            {
                if (EditorApplication.isPlaying)
                {
                    EventSystem.Instance.Invoke(new BBTestManagerCallback()
                    {
                        instanceId = testManager.instanceId, order = testManager.currentOrder, stop = 0
                    });
                }
                else
                {
                    Debug.LogError("cannot editor in edit mode");
                }
            }

            if (GUILayout.Button("Stop Behavior"))
            {
                if (EditorApplication.isPlaying)
                {
                    EventSystem.Instance.Invoke(new BBTestManagerCallback()
                    {
                        instanceId = testManager.instanceId, order = testManager.currentOrder, stop = 1
                    });
                }
                else
                {
                    Debug.LogError("cannot edit in edit mode");
                }
            }
        }
    }
}