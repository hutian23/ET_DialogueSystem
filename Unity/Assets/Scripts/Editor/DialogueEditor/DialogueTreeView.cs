using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class DialogueTreeView: GraphView
    {
        public new class UxmlFactory: UxmlFactory<DialogueTreeView, UxmlTraits>
        {
        }

        private DialogueTree tree;

        public Vector2 mousePosition;
        private SearchMenuWindowProvider searchWindow;
        
        public DialogueTreeView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            //注册鼠标移动事件
            this.RegisterCallback<MouseMoveEvent>(this.OnMouseMove);
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/DialogueEditor.uss");
            this.styleSheets.Add(styleSheet);
            
            this.AddSearchWindow();
            
        }

        private void AddSearchWindow()
        {
            this.searchWindow = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
            this.searchWindow.Init(this);
            //添加回调，按下空格调用
            this.nodeCreationRequest = context =>
            {
                //打开一个searchWindow
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), this.searchWindow);
            };
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            this.mousePosition = evt.localMousePosition;
        }
    }
}