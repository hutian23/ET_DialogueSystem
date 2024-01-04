using ET.Client;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditor: EditorWindow
{
    private DialogueTreeView treeView;
    private InspectorView inspectorView;
    private Toolbar toolbar;
    public Toggle autoSaveToggle;
    private DropdownField dropDown;
    
    public bool HasUnSave
    {
        get => hasUnsavedChanges;
        set => hasUnsavedChanges = value;
    }

    private DialogueTree tree;

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<DialogueTreeView>();
        treeView.OnNodeSelected = OnNodeSelected;

        inspectorView = root.Q<InspectorView>();

        toolbar = root.Q<Toolbar>();
        autoSaveToggle = toolbar.Q<ToolbarToggle>();

        dropDown = toolbar.Q<DropdownField>();
    }
    
    public static void OpenWindow(DialogueTree dialogueTree)
    {
        DialogueEditor wnd = GetWindow<DialogueEditor>();
        wnd.titleContent = new GUIContent("DialogueEditor");
        wnd.tree = dialogueTree;
        wnd.treeView.PopulateView(wnd.tree, wnd);
        //初始化下拉框
        wnd.LoadDropDownMenuIten();
        wnd.dropDown.SetValueWithoutNotify(wnd.tree.treeName);
    }

    public void SaveDialogueTree()
    {
        treeView.SaveCommentBlock();
        treeView.SaveNodes();
        HasUnSave = false;
        EditorUtility.SetDirty(tree);
    }

    public void OnInspectorUpdate()
    {
        if (autoSaveToggle.value && HasUnSave)
        {
            SaveDialogueTree();
        }
    }

    /// <summary>
    /// 关闭editor时窗口中save的回调
    /// </summary>
    public override void SaveChanges()
    {
        base.SaveChanges();
        SaveDialogueTree();
    }

    private void OnNodeSelected(DialogueNodeView dialogueNodeView)
    {
        inspectorView.UpdateSelection(dialogueNodeView);
    }

    private void LoadDropDownMenuIten()
    {
        dropDown.choices.Clear();
        //选择下拉菜单后的回调
        dropDown.RegisterValueChangedCallback(this.ChangeDropDown);
        string folderPath = "Assets/Res/ScriptableObject/DialogueTree";
        string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });
        foreach (var guid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof (DialogueTree));
            if (asset != null && asset is DialogueTree dialogueTree)
            {
                dropDown.choices.Add(dialogueTree.treeName);
            }
        }
    }
    
    private void ChangeDropDown(ChangeEvent<string> evt)
    {
        string folderPath = "Assets/Res/ScriptableObject/DialogueTree";
        string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });
        foreach (var guid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof (DialogueTree));
            if (asset != null && asset is DialogueTree dialogueTree && dialogueTree.treeName == evt.newValue)
            {
                OpenWindow(dialogueTree);
            }
        }
    }
}