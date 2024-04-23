using System;
using UnityEngine;
using UnityEngine.Playables;

namespace ET
{
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayUtilitySample : MonoBehaviour
    {
        public AnimationClip clip;
        public PlayableGraph playableGraph;

        private void Start()
        {
            AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), clip, out playableGraph);
        }

        private void OnDisable()
        {
            //销毁该图创建的所有可播放项和输出
            playableGraph.Destroy();
        }
    }
}
