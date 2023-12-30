using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class DialogueEditor : EditorWindow
{
    [MenuItem("Window/UI Toolkit/DialogueEditor")]
    public static void ShowExample()
    {
        DialogueEditor wnd = GetWindow<DialogueEditor>();
        wnd.titleContent = new GUIContent("DialogueEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = this.rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uss");
        root.styleSheets.Add(styleSheet);
        
        
        // // Each editor window contains a root VisualElement object
        // VisualElement root = rootVisualElement;
        //
        // // VisualElements objects can contain other VisualElement following a tree hierarchy.
        // VisualElement label = new Label("Hello World! From C#");
        // root.Add(label);
        //
        // // Import UXML
        // var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uxml");
        // VisualElement labelFromUXML = visualTree.Instantiate();
        // root.Add(labelFromUXML);
        //
        // // A stylesheet can be added to a VisualElement.
        // // The style will be applied to the VisualElement and all of its children.
        // var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uss");
        // VisualElement labelWithStyle = new Label("Hello World! With Style");
        // labelWithStyle.styleSheets.Add(styleSheet);
        // root.Add(labelWithStyle);
    }
}