using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ET
{
    public class PlayAnimationSample : MonoBehaviour
    {
        public AnimationClip clip;
        public PlayableGraph playableGraph;

        private void Start()
        {
            playableGraph = PlayableGraph.Create("hutian");
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
            //将剪辑包裹在可播放项中
            var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
            // 将可播放想连接到输出
            playableOutput.SetSourcePlayable(clipPlayable);
            //播放该图
            playableGraph.Play();
        }

        private void OnDisable()
        {
            //销毁该图创建的所有可播放项和playableOutput
            playableGraph.Destroy();
        }
    }
}
