using ImGuiNET;
using UnityEngine;

namespace ET
{
    public class Box2DTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ImGuiUn.Layout += this.OnPreRender;
        }
        
        private void OnPreRender()
        {
            Debug.LogWarning("OnPreRender");
        }

        private void OnPostRender()
        {
            Debug.LogWarning("Post Render");
        }
    }
}
