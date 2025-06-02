using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class CameraManager : Entity,IAwake,IDestroy, ILoad, IUpdate, IPostStep, IGizmosUpdate
    {
        [StaticField]
        public static CameraManager instance;
        public Camera MainCamera;
        
        //初始位置
        public Vector3 Position = new(0, 0, -10f);
        //MouseMove
        public Vector3 Difference;
        public Vector3 Origin;
        public bool Drag;

        public int _screenWidth;
        public int _screenHeight;
        
        //Zoom
        public Vector2 Scroll;
    }
}