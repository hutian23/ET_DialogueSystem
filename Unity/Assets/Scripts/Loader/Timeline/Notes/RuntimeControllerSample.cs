using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ET
{
    [RequireComponent(typeof(Animator))]
    public class RuntimeControllerSample : MonoBehaviour
    {
        public AnimationClip clip;
        public RuntimeAnimatorController controller;
        public float weight;
        public PlayableGraph playableGraph;
        public AnimationMixerPlayable mixerPlayable;

        private void Start()
        {
            //创建图和混合器，将他们绑定到Animator
            playableGraph = PlayableGraph.Create();
            var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
            mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);
            playableOutput.SetSourcePlayable(mixerPlayable);
            
            //创建AnimationClipPlayable 将他们连接到混合器
            var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
            var ctrlPlayable = AnimatorControllerPlayable.Create(playableGraph, controller);

            playableGraph.Connect(clipPlayable, 0, mixerPlayable, 0);
            playableGraph.Connect(clipPlayable, 1, mixerPlayable, 0);
            
            //播放该图
            playableGraph.Play();
        }

        [Button("test")]
        private void Test()
        {
            playableGraph = PlayableGraph.Create("hutian");
            AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
            mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);
            playableOutput.SetSourcePlayable(mixerPlayable);

            var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
            var clipPlayable2 = AnimationClipPlayable.Create(playableGraph, clip);
            
            playableGraph.Connect(clipPlayable, 0, mixerPlayable, 0);
            playableGraph.Connect(clipPlayable2, 0, mixerPlayable, 1);
        }
        
        private void Update()
        {
            weight = Mathf.Clamp01(weight);
            mixerPlayable.SetInputWeight(0, weight);
            mixerPlayable.SetInputWeight(1, weight);
        }

        private void OnDisable()
        {
            //销毁该图创建的所有可播放项和输出
            playableGraph.Destroy();
        }
    }
}
