using ET.Client;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class BehaviorParamView: VisualElement
    {
        public new class UxmlFactory: UxmlFactory<BehaviorParamView, UxmlTraits>
        {
        }

        private SharedVariable variable = new() { name = "New Paramter", value = new AnimationCurve() };
        private IMGUIContainer controllerContainer;

        public BehaviorParamView()
        {
            VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>($"VisualTree/BehaviorParamView");
            visualTreeAsset.CloneTree(this);
            AddToClassList("BehaviorParamView");
        }
    }
}