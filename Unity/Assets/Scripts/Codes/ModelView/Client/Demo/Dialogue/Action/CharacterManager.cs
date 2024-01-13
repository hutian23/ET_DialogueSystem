using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class CharacterManager: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, long> characters = new();
    }
}