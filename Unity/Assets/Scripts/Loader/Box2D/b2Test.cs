using System.Collections.Generic;
using Box2DSharp.Testbed.Unity.Inspection;
using ImGuiNET;
using UnityEngine;

namespace ET
{
    public class b2Test: MonoBehaviour
    {
        public UnityDraw draw;

        public void OnEnable()
        {
            ImGuiUn.Layout += this.RenderUI;
        }

        public void OnDisable()
        {
            ImGuiUn.Layout -= this.RenderUI;
        }

        private void RenderUI()
        {
            this.draw.PostLines(new List<(Vector3 begin, Vector3 end)> { (Vector3.zero, Vector3.one) }, Color.cyan);
        }
    }
}