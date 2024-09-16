using UnityEngine;

namespace ET
{
    public class TriggerCallback : MonoBehaviour
    {
        public long instanceId;
        public int a = 10;
        public void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(a--);
        }
    }
}
