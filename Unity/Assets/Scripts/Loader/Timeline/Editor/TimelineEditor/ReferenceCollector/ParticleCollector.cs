using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline.Editor
{
    public class TimelineGenerateAttribute: Attribute
    {
    }

    [TimelineGenerate]
    public class ParticleCollector: MonoBehaviour
    {
        [ReadOnly]
        public string particleName;

        public void Init(BBParticleClip clip)
        {
            particleName = clip.ParticleName;
            gameObject.name = particleName;
        }
    }
}