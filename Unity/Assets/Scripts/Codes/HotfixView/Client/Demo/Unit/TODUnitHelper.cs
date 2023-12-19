namespace ET.Client
{
    public static class TODUnitHelper
    {
        public static void AddPlayer(Scene clientScene, Unit player)
        {
            //移除之前的unit
            PlayerComponent playerComponent = clientScene.GetComponent<PlayerComponent>();
            playerComponent.RemoveChild(playerComponent.MyId);

            playerComponent.AddChild(player);
            playerComponent.MyId = player.Id;
        }

        /// <summary>
        /// 从ClientScene获取玩家unit
        /// </summary>
        /// <param name="clientScene"></param>
        /// <returns></returns>
        public static Unit GetPlayer(Scene clientScene)
        {
            PlayerComponent playerComponent = clientScene.GetComponent<PlayerComponent>();
            long playerId = playerComponent.MyId;
            return playerComponent.GetChild<Unit>(playerId);
        }

        public static Unit GetUnitFromCurrentScene(Scene clientScene, long unitId)
        {
            UnitComponent unitComponent = clientScene.CurrentScene().GetComponent<UnitComponent>();
            Unit unit = unitComponent.GetChild<Unit>(unitId);
            if (unit == null)
            {
                Log.Warning($"当前场景没有此unit,Id为{unitId}");
            }
            return unit;
        }
    }
}