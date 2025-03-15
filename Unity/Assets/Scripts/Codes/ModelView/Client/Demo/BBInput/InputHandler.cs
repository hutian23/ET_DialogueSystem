namespace ET.Client
{
    public class InputAttribute: BaseAttribute
    {
    }
    
    [Input]
    public abstract class InputHandler
    {
        public abstract string GetHandlerType();

        public abstract string GetBufferType();

        public abstract long Handle(InputWait self);
    }
}