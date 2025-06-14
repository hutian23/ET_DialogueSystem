using Box2DSharp.Dynamics.Contacts;

namespace ET.Event
{
    public struct AfterB2WorldCreated
    {
        public b2World B2World;
    }

    #region 碰撞回调
    
    public enum TriggerType
    {
        None = 0,
        TriggerEnter = 1,
        TriggerStay = 2,
        TriggerExit = 3
    }
    
    public struct TriggerEnterCallback
    {
        public CollisionBuffer buffer;
    }
    
    public struct TriggerExitCallback
    {
        public CollisionBuffer buffer;
    }
    
    public struct TriggerStayCallback
    {
        public CollisionBuffer buffer;
    }
    
    public struct CollisionEnterCallback
    {
        public CollisionBuffer buffer;
    }
    
    public struct CollisionStayCallback
    {
        public CollisionBuffer buffer;
    }
    
    public struct CollisionExitCallback
    {
        public CollisionBuffer buffer;
    }
    #endregion
    

    public struct CollisionBuffer
    {
        public long instanceIdA; // 
        public long instanceIdB; // 
        public Contact Contact;  // 两个夹具的接触点
    }
    
    public static class CollisionEnterType
    {
        public const int None = 0;
        public const int SceneBoxEvent = 1;
        public const int CameraEvent = 2;
        public const int HandleCallback = 3;
    }
    
    public static class CollisionExitType
    {
        public const int None = 0;
        public const int SceneBoxEvent = 1;
        public const int CameraEvent = 2;
        public const int HandleCallback = 3;
    }
    
    public static class CollisionStayType
    {
        public const int None = 0;
        public const int SceneBoxEvent = 1;
        public const int CameraEvent = 2;
        public const int CollisionEvent = 3;
        public const int HandleCallback = 4;
    }
    
    public static class TriggerEnterType
    {
        public const int None = 0;
        public const int AirCheck = 1;
        public const int CollisionEvent = 2;
        public const int SceneBoxEvent = 3;
        public const int CameraEvent = 4;
        public const int HandleCallback = 5;
    }
    
    public static class TriggerExitType
    {
        public const int None = 0;
        public const int AirCheck = 1;
        public const int SceneBoxEvent = 2;
        public const int CameraEvent = 3;
        public const int HandleCallback = 4;
    }
    
    public static class TriggerStayType
    {
        public const int None = 0;
        public const int TriggerEvent = 1;
        public const int SceneBoxEvent = 2;
        public const int CameraEvent = 3;
        public const int HandleCallback = 4;
    }
}