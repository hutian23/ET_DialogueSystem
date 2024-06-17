using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class BehaviorControllerView: GraphView
    {
        public new class UxmlFactory: UxmlFactory<BehaviorControllerView, UxmlTraits>
        {
        }

        #region Position

        private Vector2 ScreenMousePosition;

        public Vector2 LocalMousePosition
        {
            get
            {
                var mousePosition = controllerEditor.rootVisualElement.ChangeCoordinatesTo(controllerEditor.rootVisualElement.parent,
                    ScreenMousePosition - controllerEditor.position.position);
                return this.contentContainer.WorldToLocal(mousePosition);
            }
        }

        #endregion

        private BehaviorControllerEditor controllerEditor;

        public BehaviorControllerView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet =
                    AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Loader/Timeline/Editor/Resources/Style/BehaviorControllerEditor.uss");
            styleSheets.Add(styleSheet);
        }
    }
}