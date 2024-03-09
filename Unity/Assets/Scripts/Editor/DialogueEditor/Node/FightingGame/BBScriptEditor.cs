using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class BBScriptEditor: OdinEditorWindow
    {
        private BBNode target;
        private TextField textField;

        public static void Init(BBNode node)
        {
            BBScriptEditor editor = (BBScriptEditor)GetWindow(typeof (BBScriptEditor), true, "BBScriptEditor");
            editor.target = node;
            editor.textField.SetValueWithoutNotify(node.BBScript);
            editor.SetDirty(false);
            editor.Show();
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            ScrollView scrollView = new(ScrollViewMode.Vertical);

            textField = new TextField();
            textField.multiline = true;

            textField.style.flexGrow = 100;
            textField.style.minHeight = 800;
            textField.RegisterValueChangedCallback(_ => { SetDirty(); });

            scrollView.Add(textField);
            root.contentContainer.Add(scrollView);

            root.RegisterCallback<KeyDownEvent>(EditorKeyDownEvent);
        }

        public void SetDirty(bool HasUnSaved = true)
        {
            hasUnsavedChanges = HasUnSaved;
        }

        private void EditorKeyDownEvent(KeyDownEvent evt)
        {
            if (!evt.ctrlKey) return;
            switch (evt.keyCode)
            {
                case KeyCode.S:
                {
                    Save();
                    evt.StopPropagation();
                    return;
                }
            }
        }

        private void Save()
        {
            target.BBScript = textField.text;
            SetDirty(false);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            Save();
        }
    }
}