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
            this.Game = game;
        }

        public void Render()
        {
            UpdateText();
        }

        private readonly Vector4 _textColor = new Vector4(0.9f, 0.6f, 0.6f, 1);

        private void UpdateText()
        {
            if (this.Game.DebugDraw.ShowUI)
            {
                ImGui.SetNextWindowPos(new Vector2(0.0f, 0.0f));
                ImGui.SetNextWindowSize(new Vector2(Global.Camera.Width, Global.Camera.Height));
                ImGui.SetNextWindowBgAlpha(0);
                ImGui.Begin("Overlay", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar);
                ImGui.End();

                while (this.Game.DebugDraw.Texts.TryDequeue(out var text))
                {
                    ImGui.Begin("Overlay", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar);
                    ImGui.SetCursorPos(text.Position.ToUnityVector2());
                    ImGui.TextColored(_textColor, text.Text);
                    ImGui.End();
                }
            }
        }
    }
}