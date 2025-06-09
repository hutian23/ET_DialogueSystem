using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class EnhanceInputComponent : Entity, IAwake, IFrameUpdate, IDestroy
    {
        // Key: InputType  Value: buffFrame
        public Dictionary<string, int> enhanceDict = new();
    }
}