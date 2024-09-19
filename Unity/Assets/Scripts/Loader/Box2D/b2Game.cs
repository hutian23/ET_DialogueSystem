using System;
using Box2DSharp.Testbed.Unity;
using Box2DSharp.Testbed.Unity.Inspection;
using ImGuiNET;
using Testbed.Abstractions;
using UnityEngine;
using UnityEngine.InputSystem;
using Camera = UnityEngine.Camera;
namespace ET
{
    //Loader层，负责渲染形状，接收输入
    public class b2Game: MonoBehaviour
    {
        private FpsCounter fpsCounter;

        private FixedUpdate fixedUpdate;

        public UnityDraw unityDraw;

        public DebugDraw DebugDraw;

        private UnityTestSettings Settings;

        public Camera MainCamera;

        public Vector3 Difference;

        public Vector3 Origin;

        public bool Drag;

        private UnityInput UnityInput;

        private b2GUIController controller;

        public void Awake()
        {
            this.fpsCounter = new();
            this.Settings = TestSettingHelper.Load();
            Global.Settings = this.Settings;
            Global.Camera.Width = this.Settings.WindowWidth;
            Global.Camera.Height = this.Settings.WindowHeight;
            Screen.SetResolution(this.Settings.WindowWidth, this.Settings.WindowHeight, this.Settings.FullScreenMode);

            this._screenWidth = Screen.width;
            this._screenHeight = Screen.height;

            this.UnityInput = new UnityInput();
            Global.Input = this.UnityInput;

            this.unityDraw = UnityDraw.GetDraw();
            this.DebugDraw = new DebugDraw { Draw = this.unityDraw };
            Global.DebugDraw = this.DebugDraw;

            Application.quitting += () => TestSettingHelper.Save(this.Settings);

            this.fixedUpdate = new FixedUpdate(TimeSpan.FromSeconds(1 / 60d), Tick);
            this.controller = new b2GUIController(this);
            this.MainCamera = Camera.main;
        }

        public void Start()
        {
            this.fixedUpdate.Start();
        }

        private void Update()
        {
            this.fixedUpdate.Update();

            this.CheckZoom();
            this.CheckResize();
            this.CheckMouseDown();
            this.CheckMouseMove();
            this.CheckMouseUp();
        }

        private void Tick()
        {
            fpsCounter.SetFps();
        }
        
        #region RenderUI
        public Action PreRenderCallback;
        private void OnPreRender()
        {
            PreRenderCallback?.Invoke();
        }

        private void OnEnable()
        {
            ImGuiUn.Layout += this.RenderUI;
        }

        private void OnDisable()
        {
            ImGuiUn.Layout -= this.RenderUI;
        }

        private void RenderUI()
        {
            controller.Render();
            DebugDraw.DrawString(5, Global.Camera.Height - 40, $"{fpsCounter.Ms:0.0} ms");
            DebugDraw.DrawString(5, Global.Camera.Height - 20, $"{fpsCounter.Fps:F1} fps");
        }

        #endregion

        public int _screenWidth;

        public int _screenHeight;

        private FullScreenMode _mode;

        #region Zoom

        public Vector2 Scroll;

        private void CheckZoom()
        {
            var scroll = Mouse.current.scroll.ReadValue();

            //zoom out
            if (scroll.y < 0)
            {
                if (this.MainCamera.orthographicSize > 1)
                {
                    this.MainCamera.orthographicSize += 1f;
                }
                else
                {
                    this.MainCamera.orthographicSize += 0.1f;
                }

                this.Scroll = scroll;
                ScrollCallback(this.Scroll.x, this.Scroll.y);
            }

            //zoom in
            if (scroll.y > 0)
            {
                if (this.MainCamera.orthographicSize > 1)
                {
                    this.MainCamera.orthographicSize -= 1f;
                }
                else if (this.MainCamera.orthographicSize > 0.2f)
                {
                    this.MainCamera.orthographicSize -= 0.1f;
                }

                this.Scroll = scroll;
                ScrollCallback(scroll.x, scroll.y);
            }
        }

        private void ScrollCallback(double dx, double dy)
        {
            if (dy > 0)
            {
                Global.Camera.Zoom /= 1.1f;
            }
            else
            {
                Global.Camera.Zoom *= 1.1f;
            }
        }

        #endregion

        #region Resize

        //分辨率自适应
        private void CheckResize()
        {
            var w = Screen.width;
            var h = Screen.height;
            var mode = Screen.fullScreenMode;
            if (this._screenWidth != w || this._screenHeight != h || this._mode != mode)
            {
                this._screenWidth = w;
                this._screenHeight = h;
                //URP项目中可能因为渲染顺序的问题,GL渲染被清空
                GL.Viewport(new Rect(0, 0, w, h));
                ResizeWindowCallback(w, h, mode);
            }
        }

        private void ResizeWindowCallback(int width, int height, FullScreenMode fullScreenMode)
        {
            Global.Camera.Width = width;
            Global.Camera.Height = height;
            this.Settings.WindowWidth = width;
            this.Settings.WindowHeight = height;
            this.Settings.FullScreenMode = fullScreenMode;
        }

        #endregion

        #region MouseControl

        private void CheckMouseDown()
        {
            var mouse = Mouse.current;
            var mousePosition = Mouse.current.position.ReadValue();

            //Drag
            if (mouse.rightButton.isPressed)
            {
                Difference = MainCamera.ScreenToWorldPoint(mousePosition) - MainCamera.transform.position;
                if (!Drag)
                {
                    Drag = true;
                    Origin = MainCamera.ScreenToWorldPoint(mousePosition);
                }
            }
            else
            {
                Drag = false;
            }
        }

        private void CheckMouseUp()
        {
            //TODO
        }

        private void CheckMouseMove()
        {
            if (Mouse.current.rightButton.isPressed)
            {
                var delta = Mouse.current.delta.ReadValue();
                Global.Camera.Center.X -= delta.x * 0.05f * Global.Camera.Zoom;
                Global.Camera.Center.Y += delta.y * 0.05f * Global.Camera.Zoom;
            }

            if (Drag)
            {
                MainCamera.transform.position = Origin - Difference;
            }
        }

        #endregion
    }
}