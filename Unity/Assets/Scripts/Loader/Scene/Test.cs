using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class Test : MonoBehaviour
    {
        public float waveWidth;
        public float waveSpeed;
        
        [OnValueChanged("Change")]
        public int curTick;

        public void Change()
        {
            Debug.Log("Change");
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Width", waveWidth);
            propertyBlock.SetFloat("_Speed", waveSpeed);
            propertyBlock.SetInt("_CurrentTick", curTick);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
