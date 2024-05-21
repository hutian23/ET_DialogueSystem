using UnityEngine;

[ExecuteInEditMode]
public class PlayParticleSystemInEditor : MonoBehaviour
{
    public ParticleSystem particleSystem;
    
    private void Update()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Hello world");
            if (particleSystem && !particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
    }
}