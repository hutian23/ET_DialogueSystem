using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Timeline;

namespace ET.Event
{
    public struct AfterB2WorldCreated
    {
        public b2World B2World;
    }

    #region 主要是区分回调
    
    public enum TriggerType
    {
        None = 0,
        TriggerEnter = 1,
        TriggerStay = 2,
        TriggerExit = 3
    }
    
    public struct TriggerEnterCallback
    {
        public CollisionInfo info;
    }
    public struct TriggerExitCallback
    {
        public CollisionInfo info;
    }
    public struct TriggerStayCallback
    {
        public CollisionInfo info;
    }
    public struct CollisionEnterCallback
    {
        public CollisionInfo info;
    }
    public struct CollisionStayCallback
    {
        public CollisionInfo info;
    }
    public struct CollisionExitCallback
    {
        public CollisionInfo info;
    }
    #endregion
    
    public struct CollisionInfo
    {
        //碰撞事件调用者
        public Fixture fixtureA;
        public FixtureData dataA;
        //和谁发生碰撞
        public Fixture fixtureB;
        public FixtureData dataB;
        //接触点
        public Contact Contact;
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