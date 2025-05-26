using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class Injector : MonoBehaviour
    {
        public TextAsset script;

        [Button("调用Test函数"),ShowIf("CheckActive")]
        public void Test()
        {
            BBScript bbScript = this.GetComponent<BBScript>();
            EventSystem.Instance.Invoke(new InjectFunctionCallback(){instanceId = bbScript.instanceId});
        }

        private bool CheckActive()
        {
            return this.GetComponent<BBScript>().instanceId != 0;
        }
    }

    public struct InjectFunctionCallback
    {
        public long instanceId;
    }
}