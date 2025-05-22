using System;
using Sirenix.OdinInspector;
using UnityEngine;
using ET;

namespace Timeline.Editor
{
    [Serializable]
    public class HitboxMarkerInspectorData: ShowInspectorData
    {
        [LabelText("当前帧: "), ReadOnly]
        public int CurrentFrame;

        [LabelText("判定框名: ")]
        public string HitboxName;
        
        [LabelText("判定框类型: "), EnumToggleButtons]
        public HitboxType HitboxType;

        [HideReferenceObjectPicker]
        [HideLabel]
        [HideInInspector]
        public HitboxKeyframe Keyframe;

        private TimelineFieldView fieldView;
        
        [PropertySpace(5)]
        [Button("新建判定框", DirtyOnClick = false)]
        private void CreateHitbox()
        {
            if (HitboxType is HitboxType.None)
            {
                Debug.LogError($"Hitbox type should not be none!");
                return;
            }

            fieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                //1. 生成子GameObject
                GameObject parent = fieldView.EditorWindow.TimelinePlayer.gameObject.GetComponent<ReferenceCollector>().Get<GameObject>(HitboxType.ToString());
                GameObject child = new(HitboxName);
                child.transform.SetParent(parent.transform);
                child.transform.localPosition = Vector2.zero;
                child.AddComponent<TimelineObject>();
                
                //2. 添加判定框组件
                b2BoxCollider2D box = child.AddComponent<b2BoxCollider2D>();
                box.info = new BoxInfo() { hitboxType = HitboxType, boxName = HitboxName };
            }, "Create hitbox", false);
        }

        [Button("刷新", DirtyOnClick = false)]
        private void Refresh()
        {
            RuntimeHitboxTrack.GenerateHitbox(fieldView.EditorWindow.TimelinePlayer, Keyframe);
        }

        [Button("保存", DirtyOnClick = false)]
        private void Save()
        {
            fieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                TimelinePlayer timelinePlayer = fieldView.EditorWindow.TimelinePlayer;
                Keyframe.boxInfos.Clear();
                foreach (b2BoxCollider2D box in timelinePlayer.GetComponentsInChildren<b2BoxCollider2D>())
                {
                    Keyframe.boxInfos.Add(MongoHelper.Clone(box.info));
                }
            }, "Save hitbox", false);
        }

        public HitboxMarkerInspectorData(object target): base(target)
        {
            Keyframe = target as HitboxKeyframe;
            this.CurrentFrame = Keyframe.frame;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            fieldView = _fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView _fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView _fieldView)
        {
        }
    }
}