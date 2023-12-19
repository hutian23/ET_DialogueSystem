using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace ET.Client
{
    [FriendOf(typeof(Input))]
    public static class InputComponentSystem
    {
        public class InputComponentAwakeSystem : AwakeSystem<Input>
        {
            protected override void Awake(Input self)
            {
                Input.Instance = self;
                self.Operas.Clear();
            }
        }

        public class InputComponentUpdateSystem : UpdateSystem<Input>
        {
            protected override void Update(Input self)
            {
                self.Operas = InputHelper.CollectInput();
            }
        }

        public static bool CheckInput(this Input self, int operaType)
        {
            return self.Operas.Contains(operaType);
        }
    }

    public static class InputHelper
    {
        private static int TranslateToOperaCode_Pressing(string keyboard)
        {
            switch (keyboard)
            {
                case "A":
                    return OperaType.LeftMovePressing;
                case "D":
                    return OperaType.RightMovePressing;
                case "W":
                    return OperaType.UpPressing;
                case "S":
                    return OperaType.DownPressing;
                default:
                    return OperaType.None;
            }
        }

        private static int TranslateToOperaCode_Released(string keyboard)
        {
            switch (keyboard)
            {
                case "A":
                    return OperaType.LeftMoveReleased;
                case "D":
                    return OperaType.RightMoveReleased;
                case "W":
                    return OperaType.UpReleased;
                case "S":
                    return OperaType.DownReleased;
                default:
                    return OperaType.None;
            }
        }

        private static int TranslateToOperaCode_WasPressed(string keyboard)
        {
            switch (keyboard)
            {
                case "A":
                    return OperaType.LeftMoveWasPressed;
                case "D":
                    return OperaType.RightMoveWasPressed;
                case "W":
                    return OperaType.UpWasPressed;
                case "S":
                    return OperaType.DownWasPressed;
                default:
                    return OperaType.None;
            }
        }
        
        public static HashSet<int> CollectInput()
        {
            var operas = new HashSet<int>();
            Keyboard keyboard = Keyboard.current;
            foreach (var key in keyboard.allKeys)
            {
                if (key.IsPressed())
                {
                    operas.Add(TranslateToOperaCode_Pressing(key.displayName));
                }

                if (key.wasPressedThisFrame)
                {
                    operas.Add(TranslateToOperaCode_WasPressed(key.displayName));
                }

                if (key.wasReleasedThisFrame)
                {
                    operas.Add(TranslateToOperaCode_Released(key.displayName));
                }
            }
            return operas;
        }
    }
}