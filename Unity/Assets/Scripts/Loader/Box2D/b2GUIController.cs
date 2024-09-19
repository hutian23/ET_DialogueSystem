using Box2DSharp.Testbed.Unity.Inspection;
using ImGuiNET;
using Testbed.Abstractions;
using UnityEngine;

namespace ET
{
    public class b2GUIController
    {
        private readonly b2Game Game;

        public b2GUIController(b2Game game)
        {
            Game = game;
        }

        public void Render()
        {
            UpdateText();
            UpdateUI();
        }

        private readonly Vector4 _textColor = new Vector4(0.9f, 0.6f, 0.6f, 1);

        private void UpdateText()
        {
            if (this.Game.DebugDraw.ShowUI)
            {
                ImGui.SetNextWindowPos(new Vector2(0.0f, 0.0f));
                ImGui.SetNextWindowSize(new Vector2(Global.Camera.Width, Global.Camera.Height));
                ImGui.SetNextWindowBgAlpha(0);
                ImGui.Begin("Overlay",
                    ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar);
                ImGui.End();

                while (this.Game.DebugDraw.Texts.TryDequeue(out var text))
                {
                    ImGui.Begin("Overlay",
                        ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar);
                    ImGui.SetCursorPos(text.Position.ToUnityVector2());
                    ImGui.TextColored(_textColor, text.Text);
                    ImGui.End();
                }
            }
        }

        private void UpdateUI()
        {
            const int MenuWidth = 220;
            if (Game.DebugDraw.ShowUI)
            {
                //窗口MinBound
                ImGui.SetNextWindowPos(new Vector2((float)Global.Camera.Width - MenuWidth - 10, 10));
                //窗口size
                ImGui.SetNextWindowSize(new Vector2(MenuWidth, (float)Global.Camera.Height - 20));
                //ToolBar
                ImGui.Begin("Tools", ref Game.DebugDraw.ShowUI, ImGuiWindowFlags.NoResize);

                if (ImGui.BeginTabBar("b2World", ImGuiTabBarFlags.None))
                {
                    if (ImGui.BeginTabItem("b2World"))
                    {
                        ImGui.SliderInt("Vel Iters", ref Global.Settings.VelocityIterations, 0, 50);
                        ImGui.SliderInt("Pos Iters", ref Global.Settings.PositionIterations, 0, 50);
                        ImGui.SliderFloat("Hertz", ref Global.Settings.Hertz, 5.0f, 120.0f, "%.0f hz");

                        ImGui.Separator();
                        
                        ImGui.Checkbox("Sleep", ref Global.Settings.EnableSleep);
                        ImGui.Checkbox("Warm Starting", ref Global.Settings.EnableWarmStarting);
                        ImGui.Checkbox("Time of Impact", ref Global.Settings.EnableContinuous);
                        ImGui.Checkbox("Sub-Stepping", ref Global.Settings.EnableSubStepping);

                        ImGui.Separator();

                        ImGui.Checkbox("Shapes", ref Global.Settings.DrawShapes);
                        ImGui.Checkbox("Joints", ref Global.Settings.DrawJoints);
                        ImGui.Checkbox("AABBs", ref Global.Settings.DrawAABBs);
                        ImGui.Checkbox("Contact Points", ref Global.Settings.DrawContactPoints);
                        ImGui.Checkbox("Contact Normals", ref Global.Settings.DrawContactNormals);
                        ImGui.Checkbox("Contact Impulses", ref Global.Settings.DrawContactImpulse);
                        ImGui.Checkbox("Friction Impulses", ref Global.Settings.DrawFrictionImpulse);
                        ImGui.Checkbox("Center of Masses", ref Global.Settings.DrawCOMs);
                        ImGui.Checkbox("Statistics", ref Global.Settings.DrawStats);
                        ImGui.Checkbox("Profile", ref Global.Settings.DrawProfile);
                        
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("ET"))
                    {
                        //Project[0]
                        var nodeOpen = ImGui.TreeNodeEx("Project");

                        var leafNodeFlags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen;

                        if (nodeOpen)
                        {
                            ImGui.TreeNodeEx("Hot Reload", leafNodeFlags);
                            if (ImGui.IsItemClicked())
                            {
                                CodeLoader.Instance.LoadHotfix();
                                EventSystem.Instance.Load();
                                Log.Debug("hot reload success");
                            }

                            ImGui.TreePop();
                        }

                        //Hitbox[1]
                        nodeOpen = ImGui.TreeNodeEx("Hitbox");
                        if (nodeOpen)
                        {
                            ImGui.Checkbox("Hitbox", ref Global.Settings.ShowHitbox);
                            ImGui.Checkbox("Hurtbox", ref Global.Settings.ShowHurtBox);
                            ImGui.Checkbox("Throwbox", ref Global.Settings.ShowThrowBox);
                            ImGui.Checkbox("ThrowHurtbox", ref Global.Settings.ShowThrowHurtBox);
                            ImGui.Checkbox("PushBox", ref Global.Settings.ShowPushBox);
                            ImGui.Checkbox("ProximityBox", ref Global.Settings.ShowProximityBox);
                            ImGui.Checkbox("OtherBox", ref Global.Settings.ShowOtherBox);
                            ImGui.EndTabItem();
                        }
                    }

                    ImGui.EndTabBar();
                }

                ImGui.End();
            }
        }
    }
}