using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class BehaviorScriptEditor: OdinEditorWindow
    {
        private BehaviorClip behaviorClip;
        private TextField textField;

        public static void Init(BehaviorClip clip)
        {
            BehaviorScriptEditor editor = (BehaviorScriptEditor)GetWindow(typeof (BehaviorScriptEditor), true, "BehaviorScriptEditor");
            editor.behaviorClip = clip;
            editor.textField.SetValueWithoutNotify(clip.Script);
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

        private void SetDirty(bool HasUnSaved = true)
        {
            hasUnsavedChanges = HasUnSaved;
        }

        private void EditorKeyDownEvent(KeyDownEvent evt)
        {
            if (!evt.ctrlKey) return;
            switch (evt.keyCode)
            {
                case KeyCode.S:
                    Save();
                    evt.StopPropagation();
                    break;
            }
        }
        
        private void Save()
        {
            behaviorClip.Script = textField.text;
            SetDirty(false);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            Save();
        }
    }
}