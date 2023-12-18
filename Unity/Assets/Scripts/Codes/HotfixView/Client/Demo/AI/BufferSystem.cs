using System.Collections.Generic;

namespace ET.Client
{
    public static class BufferSystem
    {
        public class BufferLoadSystem : LoadSystem<Buffer>
        {
            protected override void Load(Buffer self)
            {
                List<Entity> removeList = ListComponent<Entity>.Create();
                foreach (var entity in self.Components.Values)
                {
                    removeList.Add(entity);
                }

                for (int i = 0; i < removeList.Count; i++)
                {
                    removeList[i].Dispose();
                }
            }
        }
    }
}