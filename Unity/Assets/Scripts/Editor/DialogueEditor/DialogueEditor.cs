using ET.Client;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueEditor : EditorWindow
{
    private DialogueTreeView treeView;
    private InspectorView inspectorView;
    
    [MenuItem("Tools/DialogueEditor")]
    public static void OpenWindow()
    {
        DialogueEditor wnd = GetWindow<DialogueEditor>();
        wnd.titleContent = new GUIContent("DialogueEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is DialogueTree)
        {
            OpenWindow();
            return true;
        }

        return false;
    }

    public void CreateGUI()
    {
        VisualElement root = this.rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uss");
        root.styleSheets.Add(styleSheet);

        this.treeView = root.Q<DialogueTreeView>();
        this.inspectorView = root.Q<InspectorView>();
    }

    private void OnSelectionChange()
    {
        DialogueTree tree = Selection.activeObject as DialogueTree;
        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            this.treeView.PopulateView(tree,this);
            this.inspectorView.contentContainer.Clear();
            this.inspectorView.contentContainer.Add(new Label(tree.name));
        }
    }
}