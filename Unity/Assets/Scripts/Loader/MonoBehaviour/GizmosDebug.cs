using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class GizmosDebug: MonoBehaviour
    {
        public static GizmosDebug Instance { get; private set; }

        public List<Vector3> Path;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            if (this.Path.Count < 2)
            {
                return;
            }
            for (int i = 0; i < Path.Count - 1; ++i)
            {
                Gizmos.DrawLine(Path[i], Path[i + 1]);
            }
        }
        
        public IEnumerator MyCoroutine()
        {
            Debug.Log("Coroutine started.");
        
            // 等待2秒钟
            yield return new WaitForSeconds(2f);

            Debug.Log("Coroutine resumed after 2 seconds.");

            // 等待1秒钟
            yield return new WaitForSeconds(1f);

            Debug.Log("Coroutine resumed after 1 second.");

            // 通过yield break来结束协程
            yield break;
        }
    }
}