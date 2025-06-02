using UnityEngine;

namespace ET
{
    [RequireComponent(typeof (SpriteRenderer))]
    public class CircleWaveController: MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private MaterialPropertyBlock propertyBlock;
        
        public Color waveColor = Color.white;
        public float waveWidth = 0.1f;
        public float waveSpeed = 1.0f;
        public int currentTick;

        public void Awake()
        {
            this.propertyBlock = new MaterialPropertyBlock();
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.spriteRenderer.GetPropertyBlock(this.propertyBlock);
        }

        public void WaveChange(float _waveWidth, float _waveSpeed, int _currentTick)
        {
            this.propertyBlock.SetFloat("_Width", _waveWidth);
            this.propertyBlock.SetFloat("_Speed", _waveSpeed);
            this.propertyBlock.SetColor("_WaveColor", this.waveColor);
            this.propertyBlock.SetFloat("_CurrentTick", _currentTick);
        }
    }
}
