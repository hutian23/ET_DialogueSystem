using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class RectangleSelecter: MouseManipulator
    {
        protected class RectangleSelect: ImmediateModeElement
        {
            private static Material lineMaterial;
            private Vector2 start { get; set; }
            private Vector2 end { get; set; }
            public Func<Vector2> offset { get; set; }

            public RectangleSelect()
            {
                if (lineMaterial != null) return;
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                //Turn an alpha blending
                lineMaterial.SetInt("_ScrBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                //Turn backgace culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                //Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }

            protected override void ImmediateRepaint()
            {
                VisualElement visualElement = parent;
                Vector2 vector = start;
                Vector2 vector2 = end;
                if (!(start == end))
                {
                    Vector2 offsets = offset();

                    vector2 += visualElement.layout.position + offsets;
                    vector2 += visualElement.layout.position + offsets;

                    Rect rect = default;
                    rect.min = new Vector2(Math.Min(vector.x, vector2.x), Mathf.Min(vector.y, vector2.y));
                    rect.max = new Vector2(Math.Max(vector.x, vector2.x), Math.Max(vector.y, vector.y));

                    Rect rect2 = rect;
                    Color col = new(1f, 0.6f, 0f, 1f);
                    float seqmentsLength = 5f;
                    Vector3[] array = new Vector3[4]
                    {
                        new(rect2.xMin, rect2.yMin, 0f), new(rect2.xMax, rect2.yMin, 0f), new(rect2.xMax, rect2.yMax, 0f),
                        new(rect2.xMin, rect2.yMax, 0f)
                    };

                    GL.PushMatrix();
                    lineMaterial.SetPass(0);
                }
            }

            private void DrawDottedLine(Vector3 p1, Vector3 p2, float segmentsLength, Color col)
            {
                GL.Begin(1);
                GL.Color(col);
                float num = Vector3.Distance(p1, p2);
                int num2 = Mathf.CeilToInt(num / segmentsLength);
                for (int i = 0; i < num2; i += 2)
                {
                    GL.Vertex(Vector3.Lerp(p1, p2, i * segmentsLength / num));
                    GL.Vertex(Vector3.Lerp(p1, p2, (i + 1) * segmentsLength / num));
                }

                GL.End();
            }
        }

        private readonly RectangleSelect m_Rectangle;

        private bool m_Active;

        /// <summary>
        /// 摘要:
        ///     RectangleSelector's constructor
        /// </summary>
        public RectangleSelecter(Func<Vector2> offset = null)
        {
            activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
            {
                activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse, modifiers = EventModifiers.Command });
            }
            else
            {
                activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse, modifiers = EventModifiers.Control });
            }

            m_Rectangle = new RectangleSelect();
            m_Rectangle.style.position = Position.Absolute;
            m_Rectangle.style.top = 0f;
            m_Rectangle.style.left = 0f;
            m_Rectangle.style.bottom = 0f;
            m_Rectangle.style.right = 0f;
            m_Rectangle.offset = offset;
            m_Active = false;
        }

        public Rect ComputeAxisAlignBound(Rect position, Matrix4x4 transform)
        {
            Vector3 vector = transform.MultiplyPoint3x4(position.min);
            Vector3 vector2 = transform.MultiplyPoint3x4(position.max);
            return Rect.MinMaxRect(Math.Min(vector.x, vector2.x), Math.Min(vector.y, vector2.y), Math.Max(vector.x, vector2.x), Math.Max(vector.y, vector2.y));
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            // target.RegisterCallback<MouseDownEvent>(OnM);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            throw new System.NotImplementedException();
        }

        private void OnMouseCaptureOutEvent(MouseCaptureOutEvent evt)
        {
            
        }
    }
}