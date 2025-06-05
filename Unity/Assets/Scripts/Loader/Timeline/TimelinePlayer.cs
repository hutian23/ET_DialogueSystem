using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline
{
    public sealed class TimelinePlayer: SerializedMonoBehaviour
    {
        [HideInInspector]
        public long instanceId; // timelineComponent.InstanceId
        
        [HideInInspector]
        public RuntimePlayable RuntimePlayable;
        
        [ShowIf("HasNotBindUnit")]
        public BBPlayableGraph PlayableGraph;
        
        public bool HasNotBindUnit
        {
            get => instanceId == 0;
        }
        
        public void OnDisable()
        {
            Dispose();
        }

        // ReSharper disable once Unity.RedundantEventFunction
        private void OnAnimatorMove()
        {
            //禁用AnimationClip对transform的修改
        }
        
        public void ClearTimelineGenerate()
        {
            var goSet = new HashSet<GameObject>();
            foreach (Component component in GetComponentsInChildren<Component>())
            {
                if (component is TimelineObject)
                {
                    goSet.Add(component.gameObject);
                }
            }

            foreach (GameObject go in goSet)
            {
                DestroyImmediate(go);
            }
        }
        
        public BBTimeline GetTimeline(string timelineName)
        {
            return PlayableGraph.GetTimeline(timelineName);
        }
        
        public void Init(BBTimeline timeline)
        {
            RuntimePlayable?.Dispose();
            RuntimePlayable = RuntimePlayable.Create(timeline, this);
#if UNITY_EDITOR
            RebindCallback += RuntimePlayable.Rebind;
#endif
        }
        
        public void Evaluate(int targetFrame)
        {
            RuntimePlayable.Evaluate(targetFrame);
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            RebindCallback = null;
#endif
            ClearTimelineGenerate(); 
            RuntimePlayable?.Dispose();
        }

#if UNITY_EDITOR
        public Action RebindCallback;
#endif
    }
}