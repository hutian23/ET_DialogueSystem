using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class CharacterManager: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, long> characters = new();
        public Dictionary<string, long> talkers = new();
    }

    public static class VN_Position
    {
        [StaticField]
        public static readonly Vector2 Middle = new(0, -4);
        [StaticField]
        public static readonly Vector2 Left = new(-5, -4);
        [StaticField]
        public static readonly Vector2 Right = new(5, -4);
    }
}