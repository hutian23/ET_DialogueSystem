﻿using System;
using System.Collections.Generic;
using ET.Client;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline/PlayableGraph", fileName = "BBPlayableGraph")]
    public class BBPlayableGraph: SerializedScriptableObject
    {
        public RootClip root;

        [HideReferenceObjectPicker]
        [OdinSerialize, NonSerialized]
        public List<BehaviorLayer> Layers = new();

        [HideReferenceObjectPicker]
        [OdinSerialize, NonSerialized]
        public List<SharedVariable> Parameters = new();

#if UNITY_EDITOR
        private SerializedObject SerializedController;

        public void SerializedUpdate()
        {
            SerializedController = new SerializedObject(this);
            SerializedController.Update();
        }

        public HashSet<BBTimeline> GetTimelines()
        {
            HashSet<BBTimeline> timelineSet = new();
            foreach (var layer in Layers)
            {
                foreach (var behaviorClip in layer.BehaviorClips)
                {
                    if (behaviorClip.Timeline == null)
                    {
                        continue;
                    }

                    timelineSet.Add(behaviorClip.Timeline);
                }
            }

            return timelineSet;
        }
#endif

        public BBTimeline GetByOrder(int order)
        {
            foreach (var layer in Layers)
            {
                foreach (var behaviorClip in layer.BehaviorClips)
                {
                    if (behaviorClip.Timeline == null)
                    {
                        continue;
                    }

                    if (behaviorClip.order == order)
                    {
                        return behaviorClip.Timeline;
                    }
                }
            }

            return null;
        }
    }

    [Serializable]
    public class RootClip
    {
        [TextArea(10, 30)]
        public string MainScript;
    }
}