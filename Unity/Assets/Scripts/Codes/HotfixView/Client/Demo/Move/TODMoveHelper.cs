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
                
                await TimerComponent.Instance.WaitFrameAsync(token);
            }
        }
    }
}