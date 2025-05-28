namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BehaviorInfo))]
    public class HandleMoveTypeCallback : AInvokeHandler<MoveTypeCallback>
    {
        public override void Handle(MoveTypeCallback args)
        {
            Unit unit = Root.Instance.Get(args.unitId) as Unit;
            BehaviorInfo info = Root.Instance.Get(args.infoId) as BehaviorInfo;

            if (info.moveType is not MoveType.Death) return;
            unit.GetComponent<BuffManager>().RemoveComponent<DeathAbility>();
        }
    }
}