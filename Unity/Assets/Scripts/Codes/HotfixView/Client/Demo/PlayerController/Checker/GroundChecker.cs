namespace ET.Client
{
    public class GroundChecker : CheckerHandler
    {
        public override int Execute(Unit unit, CheckerConfig config)
        {
            TODMoveComponent move = unit.GetComponent<TODMoveComponent>();
            if (move == null) return 0;
            if (move.CheckGround())
            {
                unit.AI_AddBuff<OnGround>();
            }
            else
            {
                unit.AI_RemoveBuff<OnGround>();
            }
            return 0;
        }
    }
}