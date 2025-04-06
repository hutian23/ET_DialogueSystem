namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBNumeric))]
    [FriendOf(typeof(NumericCallback))]
    [FriendOf(typeof(BBParser))]
    public class HandleBBNumericChangedCallback : AInvokeHandler<BBNumericChangedCallback>
    {
        public override void Handle(BBNumericChangedCallback args)
        {
            BBNumeric numeric = Root.Instance.Get(args.instanceId) as BBNumeric;
            Unit unit = numeric.GetParent<Unit>();
            BBParser parser = unit.GetComponent<BBParser>();

            //1. 找到对应回调
            if (!numeric.NumericCallbackDict.TryGetValue(args.numericType, out long instanceId))
            {
                return;
            }
            NumericCallback callback = Root.Instance.Get(instanceId) as NumericCallback;
            int startIndex = callback.startIndex;
            int endIndex = callback.endIndex;

            //2. 执行回调
            parser.RegistParam("Numeric_NumericType", callback.NumericType);
            parser.RegistParam("Numeric_OldValue", args.oldValue);
            parser.RegistParam("Numeric_NewValue", args.newValue);
            parser.RegistSubCoroutine(startIndex, endIndex, parser.CancellationToken).Coroutine();
            parser.TryRemoveParam("Numeric_NumericType");
            parser.TryRemoveParam("Numeric_OldValue");
            parser.TryRemoveParam("Numeric_NewValue");
        }
    }
}