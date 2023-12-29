using UnityEngine;

namespace ET.Client
{
    public static class TODMoveHelper
    {
        public static async ETTask FallCor(this Unit unit, ETCancellationToken token)
        {
            if (unit == null || unit.IsDisposed || unit.GetComponent<TODMoveComponent>() == null)
            {
                return;
            }

            while (true)
            {
                if (token.IsCancel()) break;
                if (!unit.AI_ContainBuff<OnGround>())
                {
                    unit.MoveY(Mathf.MoveTowards(unit.GetSpeed().y, Constants.MaxFall, Constants.Gravity));
                }
                unit.MoveY(0f);
                await TimerComponent.Instance.WaitFrameAsync(token);
            }
        }
    }
}