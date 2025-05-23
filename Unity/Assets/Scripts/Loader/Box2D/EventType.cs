using Box2DSharp.Dynamics.Contacts;

namespace ET.Event
{
    public struct ContactFilterCallback
    {
        public long InstanceIdA;
        public long InstanceIdB;
    }
    
    public struct PreStepCallback
    {
        
    }
    
    public struct PostStepCallback
    {
    }

    public struct BeginContactCallback
    {
        public Contact Contact;
    }

    public struct EndContactCallback
    {
        public Contact Contact;
    }
    
    public struct PreSolveCallback
    {
        public Contact Contact;
    }

    public struct UpdateFlipCallback
    {
        public long instanceId;
        public int flip;
    }
}