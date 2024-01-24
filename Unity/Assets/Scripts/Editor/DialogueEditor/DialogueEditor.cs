using ET;
using ET.Client;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditor: OdinEditorWindow
{
    private DialogueTreeView treeView;
    public InspectorView inspectorView;
    private Toolbar toolbar;

    public Toggle autoSaveToggle;

    public bool HasUnSave
    {
        get => hasUnsavedChanges;
        set => hasUnsavedChanges = value;
    }

    private DialogueTree tree;

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/DialogueEditor/Resource/DialogueEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/Resource/DialogueEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<DialogueTreeView>();
        inspectorView = root.Q<InspectorView>();

        toolbar = root.Q<Toolbar>();
        autoSaveToggle = toolbar.Q<ToolbarToggle>();
    }

    public static void OpenWindow(DialogueTree dialogueTree)
    {
        DialogueEditor wnd = GetWindow<DialogueEditor>();
        wnd.titleContent = new GUIContent("DialogueEditor");
        wnd.tree = dialogueTree;
        wnd.treeView.PopulateView(wnd.tree, wnd);
    }

    public void OnInspectorUpdate()
    {
        if (autoSaveToggle.value && HasUnSave)
        {
            treeView.SaveDialogueTree();
        }

        treeView.RefreshNodeState();
    }

    /// <summary>
    /// 关闭editor时窗口中save的回调
    /// </summary>
    public override void SaveChanges()
    {
        base.SaveChanges();
        treeView.SaveDialogueTree();
    }
}