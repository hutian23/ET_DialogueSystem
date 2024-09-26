namespace ET.Client
{
    public static class TODUnitFactory
    {
        public static Unit CreatePlayer(Scene clientScene)
        {
            PlayerComponent playerComponent = clientScene.GetComponent<PlayerComponent>();
            //如果存在unit,则移除
            playerComponent.RemoveChild(playerComponent.MyId);
            Unit player = playerComponent.AddChild<Unit, int>(1001);
            playerComponent.MyId = player.Id;
            return player;
        }

        public static Unit Create(Scene currentScene)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChild<Unit, int>(1001);
            return unit;
        }
    }
}