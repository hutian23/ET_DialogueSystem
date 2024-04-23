using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ET
{
    public class PauseSubGraphAnimationSample : MonoBehaviour
    {
        public AnimationClip clip;
        public float time;
        public PlayableGraph playableGraph;
        public AnimationClipPlayable playableClip;

        void Start()
        {
            playableGraph = PlayableGraph.Create();
            var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
            
            playableClip = AnimationClipPlayable.Create(playableGraph,clip);
            playableOutput.SetSourcePlayable(playableClip);
            
            playableGraph.Play();
            playableClip.Pause();
        }

        void Update()
        {
            time += Time.deltaTime;
            //手动控制时间
            playableClip.SetTime(time);
        }

        private void OnDisable()
        {
            //销毁该图
            playableGraph.Destroy();
        }
    }
}
