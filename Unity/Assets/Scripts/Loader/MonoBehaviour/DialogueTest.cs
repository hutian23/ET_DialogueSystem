using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class DialogueTest : MonoBehaviour
    {
        [TextArea]
        public string desc;
        
        [ContextMenu("dialogueTest")]
        public void Test()
        {
            Debug.Log("Hello world");
        }
    }
}
