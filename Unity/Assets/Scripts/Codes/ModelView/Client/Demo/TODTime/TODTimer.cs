namespace ET.Client
{
    public abstract class TODTimer<T>: AInvokeHandler<TODTimerCallback> where T : class
    {
        public override void Handle(TODTimerCallback a)
        {
            this.Run(a.Args as T);
        }

        protected abstract void Run(T t);
    }
}