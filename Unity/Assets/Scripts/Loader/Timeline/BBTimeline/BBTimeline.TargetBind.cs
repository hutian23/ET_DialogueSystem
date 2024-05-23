using System;
using System.Collections.Generic;
using ET;
using Timeline.Editor;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    [BBTrack("TargetBind")]
#if UNITY_EDITOR
    [Color(100, 100, 100)]
    [IconGuid("51d6e4824d3138c4880ca6308fa0e473")]
#endif
    public class BBTargetBindTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeTargetBindTrack);

#if UNITY_EDITOR
        protected override Type ClipType => typeof (BBTargetBindClip);
        public override Type ClipViewType => typeof (TargetBindClipView);
#endif
    }

#if UNITY_EDITOR
    [Color(100, 100, 100)]
#endif
    public class BBTargetBindClip: BBClip
    {
        public bool InSceneView;
        public string referName;
        public Dictionary<int, TargetKeyframe> TargetKeyframeDict = new();

        public BBTargetBindClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (BBTargetBindInspectorData);
#endif
    }

    public class TargetKeyframe
    {
        public Vector3 offset;
        public Vector3 rotation;
    }

    #region Runtime

    public class RuntimeTargetBindTrack: RuntimeTrack
    {
        private int currentFrame;

        public RuntimeTargetBindTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
        }

        public override void SetTime(int targetFrame)
        {
            if (currentFrame == targetFrame) return;
            currentFrame = targetFrame;
        }

        public override void RuntimMute(bool value)
        {
        }
    }

    #endregion

    #region Editor

    [Serializable]
    public class BBTargetBindInspectorData: ShowInspectorData
    {
        private BBTargetBindClip targetBindClip;
        private TimelineFieldView FieldView;
        private TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        private TimelinePlayer timelinePlayer => EditorWindow.TimelinePlayer;

        public bool InSceneView;

        public bool IsRefer => InSceneView;
        public bool IsBundle => !InSceneView;

        [Sirenix.OdinInspector.ShowIf("IsRefer")]
        public GameObject referGameObject;

        [Sirenix.OdinInspector.InfoBox("注意需要添加abundle标签")]
        [Sirenix.OdinInspector.ShowIf("IsBundle")]
        public GameObject Prefab;

        [Sirenix.OdinInspector.Button("Rebind Target")]
        public void Rebind()
        {
            EditorWindow.ApplyModify(() =>
            {
                if (IsBundle)
                {
                    if (Prefab == null) return;
                    string path = AssetDatabase.GetAssetPath(Prefab);
                    AssetImporter importer = AssetImporter.GetAtPath(path);
                    targetBindClip.referName = importer.assetBundleName;
                }
                else if (IsRefer)
                {
                    if (referGameObject == null) return;
                    ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
                    refer.Remove(referGameObject.name);
                    refer.Add(referGameObject.name, referGameObject);
                    targetBindClip.referName = referGameObject.name;
                }

                targetBindClip.InSceneView = InSceneView;
            }, "targetbind rebind");
        }

        public BBTargetBindInspectorData(object target): base(target)
        {
            targetBindClip = target as BBTargetBindClip;
            InSceneView = targetBindClip.InSceneView;

            if (IsRefer)
            {
            }
            else if (IsBundle)
            {
                string path = Define.GetAssetPathsFromAssetBundle(targetBindClip.referName)[0];
                Prefab = Define.LoadAssetAtPath(path) as GameObject;
            }
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
        }
    }

    #endregion
}