using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class HitboxClipInfo
    {
        [LabelText("当前帧")]
        public int Frame;

        [LabelText("绑定对象")]
        public GameObject bindGo;

        [Space(10)]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<HitboxInfo> HitboxInfos = new();
    }
    
    public class HitboxClipView: TimelineClipView
    {
        protected override void MenuBuilder(DropdownMenu menu)
        {
            base.MenuBuilder(menu);
        }

        VisualElement ClipInspector => FieldView.ClipInspector;
        
        public override void PopulateInspector()
        {
            HitboxClipInfo clipInfo = new HitboxClipInfo();
            HitboxInspectorView inspectorView = ScriptableObject.CreateInstance<HitboxInspectorView>();
            inspectorView.info = clipInfo;
            
            var editor = UnityEditor.Editor.CreateEditor(inspectorView);
            IMGUIContainer container = new(() => { editor.OnInspectorGUI(); });
            ClipInspector.Add(container);
        }
    }

    public class HitboxInspectorView: SerializedScriptableObject
    {
        [HideReferenceObjectPicker]
        public HitboxClipInfo info;
    }
}