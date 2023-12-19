using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (GlobalComponent))]
    public static class ReferenceHelper
    {
        public static T GetGlobalRC<T>(string name) where T : class
        {
            ReferenceCollector rc = GlobalComponent.Instance.Global.GetComponent<ReferenceCollector>();
            return rc.Get<T>(name);
        }

        public static T GetRC<T>(this Unit unit, string name) where T : class
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Error("unit doesn't exist");
                return null;
            }

            GameObject go = unit.GetComponent<GameObjectComponent>()?.GameObject;
            if (go == null)
            {
                Log.Error($"please add gameObjectComponent to Unit: {unit.InstanceId}");
                return null;
            }

            ReferenceCollector rc = go.GetComponent<ReferenceCollector>();
            if (rc == null)
            {
                Log.Error($"please add rc to gameobject: {go.name}");
                return null;
            }

            return rc.Get<T>(name);
        }
    }
}