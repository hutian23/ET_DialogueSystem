using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ET
{
    public class MixAnimationSample: MonoBehaviour
    {
        public AnimationClip clip0;
        public AnimationClip clip1;

        public float weight;
        public PlayableGraph playableGraph;
        public AnimationMixerPlayable mixerPlayable;

        private void Start()
        {
            //创建改图和混合器，将他们绑定到Animator上
            playableGraph = PlayableGraph.Create("Mixer_Test");

            var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
            mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);

            playableOutput.SetSourcePlayable(mixerPlayable);

            //创建AnimationClipPlayable 并将他们连接到混合器
            var clipPlayable0 = AnimationClipPlayable.Create(playableGraph, clip0);
            var clipPlayable1 = AnimationClipPlayable.Create(playableGraph, clip1);

            playableGraph.Connect(clipPlayable0, 0, mixerPlayable, 0);
            playableGraph.Connect(clipPlayable1, 0, mixerPlayable, 1);

            //播放该图
            playableGraph.Play();
        }

        public void Update()
        {
            weight = Mathf.Clamp01(weight);
            mixerPlayable.SetInputWeight(0, 1 - weight);
            mixerPlayable.SetInputWeight(1, weight);
        }

        private void OnDisable()
        {
            //销毁该图创建的所有可播放项和输出
            playableGraph.Destroy();
        }
    }
}