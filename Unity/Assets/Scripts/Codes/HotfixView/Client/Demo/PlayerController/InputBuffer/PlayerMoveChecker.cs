namespace ET.Client
{
    [FriendOf(typeof (Move))]
    public class PlayerMoveChecker: CheckerHandler
    {
        public override int Execute(Unit unit, CheckerConfig config)
        {
            if (!unit.AI_ContainBuff<Move>())
            {
                unit.AI_AddBuff<Move>();
            }

            Move movebuffer = unit.AI_GetBuff<Move>();

            //这里我们希望，同时按下左右不移动
            //水平输入
            int count = (Input.Instance.CheckInput(OperaType.RightMovePressing)? 1 : 0) +
                    (Input.Instance.CheckInput(OperaType.LeftMovePressing)? 1 : 0);
            if (count != 1)
            {
                movebuffer.move.x = 0;
            }
            else
            {
                movebuffer.move.x = Input.Instance.CheckInput(OperaType.RightMovePressing)? 1 : -1;
            }

            //竖直输入
            count = (Input.Instance.CheckInput(OperaType.UpPressing)? 1 : 0) + (Input.Instance.CheckInput(OperaType.DownPressing)? 1 : 0);
            if (count != 1)
            {
                movebuffer.move.y = 0;
            }
            else
            {
                movebuffer.move.y = Input.Instance.CheckInput(OperaType.UpPressing)? 1 : -1;
            }
            return 0;
        }
    }
}