namespace ET.Client
{
    public abstract class TODTimer<T>: AInvokeHandler<BBTimerCallback> where T : class
    {
        public override void Handle(BBTimerCallback a)
        {
            this.Run(a.Args as T);
        }

        protected abstract void Run(T t);
    }
}