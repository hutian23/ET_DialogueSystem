﻿using System;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    [BBTrack("SubTimeline")]
#if UNITY_EDITOR
    [Color(100, 100, 100)]
    [IconGuid("799823b53d556d34faeb55e049c91845")]
#endif
    public class SubTimelineTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeSubTimelineTrack);

#if UNITY_EDITOR
        protected override Type ClipType => typeof (SubTimelineClip);
        public override Type ClipViewType => typeof (SubTimelineClipView);
#endif
    }

    [Color(100, 100, 100)]
    public class SubTimelineClip: BBClip
    {
        public int BehaviorOrder;
        public string targetBind;

        public SubTimelineClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (BBSubTimelineInspectorData);

        //So不能保存对scene gameobject的引用，只记录gameobject name
        public string testBinder;
#endif
    }

    #region Editor

    [Serializable]
    public class BBSubTimelineInspectorData: ShowInspectorData
    {
        [InfoBox("编辑器阶段使用")]
        public TimelinePlayer TestBinder;

        [InfoBox("未绑定则默认为root transform")]
        public TargetBindCollector targetBind;

        public int Order;

        [Button("Rebind")]
        public void Bind()
        {
            EditorWindow.ApplyModify(() =>
            {
                subTimelineClip.BehaviorOrder = Order;
                subTimelineClip.targetBind = targetBind == null? string.Empty : targetBind.targetBindName;
                subTimelineClip.testBinder = TestBinder == null? string.Empty : TestBinder.transform.gameObject.GetFullPath();
            }, "Update subTimelineClip");
        }

        private bool hasTestBinder => TestBinder != null;

        [Button("Open TimelineEditorWindow"), ShowIf("hasTestBinder")]
        public void OpenTimeline()
        {
            // if (!TestBinder.BBPlayable.Timelines.TryGetValue(Order, out BBTimeline timeline))
            // {
            //     Debug.LogError($"not exist timeline,order:{Order}");
            //     return;
            // }
            //
            // TestBinder.ClearTimelineGenerate();
            // TimelineEditorWindow window = ScriptableObject.CreateInstance<TimelineEditorWindow>();
            // window.Show();
            // window.TimelinePlayer = TestBinder;
            // window.TimelinePlayer.Dispose();
            // window.TimelinePlayer.Init(timeline);
            // window.PopulateView();
        }

        private TimelineFieldView fieldView;
        private TimelineEditorWindow EditorWindow => fieldView.EditorWindow;
        private SubTimelineClip subTimelineClip;

        public BBSubTimelineInspectorData(object target): base(target)
        {
            subTimelineClip = target as SubTimelineClip;
        }

        private void UpdateProperty()
        {
            Order = subTimelineClip.BehaviorOrder;
            //根据引用找到对应go
            if (!string.IsNullOrEmpty(subTimelineClip.testBinder))
            {
                Transform referTrans = EditorWindow.TimelinePlayer.transform.root.Find(subTimelineClip.testBinder);
                if (referTrans != null) TestBinder = referTrans.GetComponent<TimelinePlayer>();
            }

            foreach (var collector in EditorWindow.TimelinePlayer.GetComponentsInChildren<TargetBindCollector>())
            {
                if (collector.targetBindName != subTimelineClip.targetBind) continue;
                targetBind = collector;
                return;
            }

            targetBind = null;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            fieldView = _fieldView;
            UpdateProperty();
        }

        public override void InspectorUpdate(TimelineFieldView _fieldView)
        {
            UpdateProperty();
        }

        public override void InspectorDestroy(TimelineFieldView _fieldView)
        {
        }
    }

    #endregion

    #region Runtime

    public class RuntimeSubTimelineTrack: RuntimeTrack
    {
        public RuntimeSubTimelineTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
            currentClip = null;
            runtimeClip?.Dispose();
        }

        private RuntimeSubtimelineClip runtimeClip;
        private SubTimelineClip currentClip;

        public override void SetTime(int targetFrame)
        {
            foreach (var clip in Track.Clips)
            {
                if (clip.InMiddle(targetFrame))
                {
                    //Update runtimeClip
                    if (currentClip == clip)
                    {
                        runtimeClip.Evaluate(targetFrame);
                    }

                    //new runtimeClip
                    runtimeClip?.Dispose();
                    currentClip = clip as SubTimelineClip;
                    runtimeClip = new RuntimeSubtimelineClip(currentClip, RuntimePlayable);
                    runtimeClip.Evaluate(targetFrame);

                    return;
                }
            }

            currentClip = null;
        }
    }

    public class RuntimeSubtimelineClip
    {
        private readonly SubTimelineClip clip;
        private readonly TimelinePlayer testBinder;
        private readonly RuntimePlayable runtimePlayable;

        public RuntimeSubtimelineClip(SubTimelineClip subClip, RuntimePlayable _runtimePlayable)
        {
            clip = subClip;
            runtimePlayable = _runtimePlayable;

            //未进行绑定
            Transform referTran = runtimePlayable.TimelinePlayer.transform.root.Find(subClip.testBinder);
            if (referTran == null) return;
            testBinder = referTran.GetComponent<TimelinePlayer>();
            if (testBinder == null) return;

            BBTimeline currentTimeline = testBinder.GetByOrder(subClip.BehaviorOrder);
            if (currentTimeline == null) return;
            testBinder.Dispose();
            testBinder.Init(currentTimeline);

            EditorApplication.update += UpdatePos;
        }

        public void Evaluate(int targetFrame)
        {
            if (testBinder == null) return;
            int clipInFrame = targetFrame - clip.StartFrame;
            testBinder.RuntimeimePlayable.Evaluate(clipInFrame);
        }

        public void Dispose()
        {
            if (testBinder == null) return;
            testBinder.Dispose();

            EditorApplication.update -= UpdatePos;
        }

        private void UpdatePos()
        {
            if (testBinder == null) return;
            foreach (var collector in runtimePlayable.TimelinePlayer.GetComponentsInChildren<TargetBindCollector>())
            {
                if (collector.targetBindName != clip.targetBind) continue;
                testBinder.transform.position = collector.transform.position;
                return;
            }
        }
    }

    #endregion
}