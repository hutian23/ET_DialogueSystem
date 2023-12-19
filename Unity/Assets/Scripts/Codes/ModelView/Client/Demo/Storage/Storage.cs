using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class Storage : Entity,IAwake,IDestroy
    {
        public readonly string path = Application.streamingAssetsPath;

        [StaticField]
        public static Storage Instance;
    }
}