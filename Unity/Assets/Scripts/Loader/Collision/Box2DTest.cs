using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class Box2DTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            Debug.LogWarning("Hello owrld");
        }

        private void OnPreRender()
        {
            Debug.LogWarning("Hello world");
        }
    }
}
