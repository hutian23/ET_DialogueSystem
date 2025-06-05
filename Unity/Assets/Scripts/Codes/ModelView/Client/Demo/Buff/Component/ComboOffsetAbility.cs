using System.Collections.Generic;
 
 namespace ET.Client
 {
     [ComponentOf(typeof(BuffManager))]
     public class ComboOffsetAbility : Entity, IAwake, IFrameUpdate, IDestroy
     {
         public Queue<ComboOffsetBuffer> bufferQueue = new();
         public ETCancellationToken token;
     }
 
     public struct ComboOffsetBuffer
     {
         public string behaviorName;
         public int buffFrame;
         public int cnt;
     }
 }