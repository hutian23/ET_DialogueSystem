namespace ET.Client
{
    public class ActorAttribute: BaseAttribute
    {
    }
    
    [Actor]
    public abstract class ActorHandler
    {
        public abstract void UpdateCollideX(TODMoveComponent move, float movex);

        public abstract void UpdateCollideY(TODMoveComponent move, float movey);
    }

    public class SolidAttribute: BaseAttribute
    {
    }

    [Solid]
    public abstract class SolidHandler
    {
        public abstract void UpdateCollideX(float movex);

        public abstract void UpdateCollideY(float movey);
    }
}