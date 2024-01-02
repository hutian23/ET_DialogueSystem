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
    private Toggle autoSaveToggle;
    private Button SaveBtn;
    
    public bool HasUnSave
    {
        get => this.hasUnsavedChanges;
        set => this.hasUnsavedChanges = value;
    }

    private DialogueTree tree;

    public void CreateGUI()
    {
        Undo.undoRedoPerformed -= this.OnRedo;
        Undo.undoRedoPerformed += this.OnRedo;
        
        VisualElement root = this.rootVisualElement;
        
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uss");
        root.styleSheets.Add(styleSheet);

        this.treeView = root.Q<DialogueTreeView>();
        this.treeView.OnNodeSelected = this.OnNodeSelected;
        
        this.inspectorView = root.Q<InspectorView>();
        this.toolbar = root.Q<Toolbar>();
        this.autoSaveToggle = this.toolbar.Q<ToolbarToggle>();
        this.SaveBtn = this.toolbar.Q<Button>();
        this.SaveBtn.clicked += this.SaveDialogueTree;
    }

    public void OnDisable()
    {
        Undo.undoRedoPerformed -= this.OnRedo;
    }

    private void OnRedo()
    {
        if (this.tree == null) return;
        Debug.Log("redo");
        this.HasUnSave = false;
        Undo.PerformRedo();
        this.treeView.PopulateView(this.tree,this);
    }

    public static void OpenWindow(DialogueTree dialogueTree)
    {
        DialogueEditor wnd = GetWindow<DialogueEditor>();
        wnd.titleContent = new GUIContent("DialogueEditor");
        wnd.tree = dialogueTree;
        
        wnd.treeView.PopulateView(wnd.tree,wnd);
    }
    
    public void SaveDialogueTree()
    {
        this.treeView.SaveCommentBlock();
        this.treeView.SaveNodes();
        this.HasUnSave = false;
        EditorUtility.SetDirty(this.tree);
        Undo.RecordObject(this.tree,"dialoguetree");
    }

    public void OnInspectorUpdate()
    {
        if (this.autoSaveToggle.value && this.HasUnSave)
        {
            this.SaveDialogueTree();
        }
    }

    /// <summary>
    /// 关闭editor时窗口中save的回调
    /// </summary>
    public override void SaveChanges()
    {
        base.SaveChanges();
        this.SaveDialogueTree();
    }
    
    private void OnNodeSelected(DialogueNodeView dialogueNodeView)
    {
        this.inspectorView.UpdateSelection(dialogueNodeView);
    }
}