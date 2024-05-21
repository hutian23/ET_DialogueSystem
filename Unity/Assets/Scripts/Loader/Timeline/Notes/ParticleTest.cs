using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class ParticleTest: MonoBehaviour
    {
        public ParticleSystem particle;

        // [Sirenix.OdinInspector.Button("测试")]
        // public void TestPlay()
        // {
        //     particle.PlayInEditor();
        // }

        [Range(0,10)]
        [OnValueChanged("Test")]
        public float Time;

        public void Test()
        {
            particle.Simulate(Time);
        }
    }
}