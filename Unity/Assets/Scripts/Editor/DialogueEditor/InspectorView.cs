using UnityEditor;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory: UxmlFactory<InspectorView, UxmlTraits>{}

        private Editor editor;
        
        
    }
}