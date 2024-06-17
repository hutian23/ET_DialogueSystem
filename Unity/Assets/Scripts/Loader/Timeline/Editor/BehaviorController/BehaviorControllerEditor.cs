using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorControllerEditor : EditorWindow
{
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/BehaviorControllerEditor");
        visualTree.CloneTree(root);
    }
}
