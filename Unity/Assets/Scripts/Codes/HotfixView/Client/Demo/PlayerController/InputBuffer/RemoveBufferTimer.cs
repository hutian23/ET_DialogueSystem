namespace ET.Client
{
    [Invoke(TimerInvokeType.RemoveBufferTimer)]
    public class RemoveBufferTimer: ATimer<Entity>
    {
        protected override void Run(Entity buffer)
        {
            if (buffer == null || buffer.IsDisposed) return;
            buffer.Dispose();
        }
    }
}