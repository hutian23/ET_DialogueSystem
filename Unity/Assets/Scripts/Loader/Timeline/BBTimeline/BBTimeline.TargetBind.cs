using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Timeline.Editor;
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
        public string referName;
        public Dictionary<int, Vector3> TargetKeyframeDict = new();

        public BBTargetBindClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (BBTargetBindInspectorData);
#endif
    }

    #region Runtime

    public class RuntimeTargetBindTrack: RuntimeTrack
    {
        private int currentFrame;

        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;

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

            foreach (BBClip clip in Track.Clips)
            {
                if (!clip.InMiddle(targetFrame)) continue;
                BBTargetBindClip targetBindClip = clip as BBTargetBindClip;

                //有无关键帧
                int clipInFrame = currentFrame - targetBindClip.StartFrame;
                if (!targetBindClip.TargetKeyframeDict.TryGetValue(clipInFrame, out var localPos)) return;

                GameObject referGo = timelinePlayer.GetComponent<ReferenceCollector>().Get<GameObject>(targetBindClip.referName);
                referGo.transform.localPosition = localPos;
            }
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
        private bool BindGo => ReferGameObject != null;

        [PropertyOrder(1)]
        public GameObject ReferGameObject;

        [PropertyOrder(2), Sirenix.OdinInspector.Button("Rebind"), Sirenix.OdinInspector.ShowIf("BindGo")]
        public void Rebind()
        {
            EditorWindow.ApplyModify(() =>
            {
                ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>() ??
                        timelinePlayer.gameObject.AddComponent<ReferenceCollector>();
                refer.Remove(ReferGameObject.name);
                refer.Add(ReferGameObject.name, ReferGameObject);
                targetBindClip.referName = ReferGameObject.name;
            }, "");
        }

        [PropertyOrder(3), Sirenix.OdinInspector.Button("Record"), Sirenix.OdinInspector.ShowIf("BindGo")]
        public void Record()
        {
            EditorWindow.ApplyModify(() =>
            {
                int clipInFrame = FieldView.GetCurrentTimeLocator() - targetBindClip.StartFrame;
                Vector3 localPos = ReferGameObject.transform.localPosition;
                targetBindClip.TargetKeyframeDict.Remove(clipInFrame);
                targetBindClip.TargetKeyframeDict.Add(clipInFrame, localPos);
            }, "");
        }

        // [PropertyOrder(1)]
        // public bool InSceneView;
        //
        // public bool IsRefer => InSceneView;
        // public bool IsBundle => !InSceneView;
        //
        // [PropertyOrder(2)]
        // [Sirenix.OdinInspector.ShowIf("IsRefer")]
        // public GameObject referGameObject;
        //
        // [PropertyOrder(2)]
        // [InfoBox("注意需要添加abundle标签")]
        // [Sirenix.OdinInspector.ShowIf("IsBundle")]
        // public GameObject Prefab;
        //
        // [PropertyOrder(3)]
        // [Sirenix.OdinInspector.Button("Rebind Target")]
        // public void Rebind()
        // {
        //     EditorWindow.ApplyModify(() =>
        //     {
        //         if (IsBundle)
        //         {
        //             if (Prefab == null) return;
        //             string path = AssetDatabase.GetAssetPath(Prefab);
        //             AssetImporter importer = AssetImporter.GetAtPath(path);
        //             if (string.IsNullOrEmpty(importer.assetBundleName))
        //             {
        //                 Debug.LogError($"please add abundle tag to prefab: {Prefab.name}");
        //                 return;
        //             }
        //
        //             targetBindClip.referName = importer.assetBundleName;
        //         }
        //         else if (IsRefer)
        //         {
        //             if (referGameObject == null) return;
        //             if (PrefabUtility.IsPartOfPrefabAsset(referGameObject))
        //             {
        //                 Debug.LogError("can not add prefab to refercollector");
        //                 return;
        //             }
        //
        //             ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
        //             refer.Remove(referGameObject.name);
        //             refer.Add(referGameObject.name, referGameObject);
        //             targetBindClip.referName = referGameObject.name;
        //         }
        //
        //         targetBindClip.InSceneView = InSceneView;
        //     }, "targetbind rebind");
        // }
        //
        //
        //
        // [PropertyOrder(4)]
        // [Sirenix.OdinInspector.ReadOnly]
        // // [Sirenix.OdinInspector.ShowIf("")]
        // public Vector3 Offset;
        //
        // [PropertyOrder(5)]
        // [Sirenix.OdinInspector.ReadOnly]
        // // [Sirenix.OdinInspector.ShowIf("")]
        // public Vector3 Rotation;
        //
        // public BBTargetBindInspectorData(object target): base(target)
        // {
        //     targetBindClip = target as BBTargetBindClip;
        //     // InSceneView = targetBindClip.InSceneView;
        // }
        //
        // public override void InspectorAwake(TimelineFieldView fieldView)
        // {
        //     FieldView = fieldView;
        //
        //     //还没绑定gameobject
        //     if (string.IsNullOrEmpty(targetBindClip.referName)) return;
        //     if (IsRefer)
        //     {
        //         ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
        //         referGameObject = refer.Get<GameObject>(targetBindClip.referName);
        //     }
        //     else if (IsBundle)
        //     {
        //         string path = Define.GetAssetPathsFromAssetBundle(targetBindClip.referName)[0];
        //         Prefab = Define.LoadAssetAtPath(path) as GameObject;
        //     }
        // }
        //
        // public override void InspectorUpdate(TimelineFieldView fieldView)
        // {
        // }
        //
        // public override void InspectorDestroy(TimelineFieldView fieldView)
        // {
        // }
        public BBTargetBindInspectorData(object target): base(target)
        {
            targetBindClip = target as BBTargetBindClip;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
            ReferGameObject = timelinePlayer.GetComponent<ReferenceCollector>().Get<GameObject>(targetBindClip.referName);
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