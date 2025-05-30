using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class Injector : MonoBehaviour
    {
        [Range(0, 240), OnValueChanged("UpdateHertz")]
        public int hertz;
        
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

        public void UpdateHertz()
        {
            BBScript bbScript = this.GetComponent<BBScript>();
            EventSystem.Instance.Invoke(new UpdateHertzCallback(){ instanceId = bbScript.instanceId, Hertz = hertz});
        }
    }

    public struct InjectFunctionCallback
    {
        public long instanceId;
    }
    
    public struct UpdateHertzCallback
    {
        public long instanceId;
        public int Hertz;
    }
}