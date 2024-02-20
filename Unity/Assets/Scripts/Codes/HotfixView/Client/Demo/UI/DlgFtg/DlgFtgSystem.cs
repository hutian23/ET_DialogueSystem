namespace ET.Client
{
    [FriendOf(typeof (DlgFtg))]
    public static class DlgFtgSystem
    {
        public static void RegisterUIEvent(this DlgFtg self)
        {
        }

        public static void ShowWindow(this DlgFtg self, Entity contextData = null)
        {
        }

        public static void Refresh(this DlgFtg self, long ops)
        {
            float disable = 0.3f, enable = 1;
            self.View.E_Arrow_DownImage.Setalpha((ops & TODOperaType.DOWN) != 0? enable : disable);
            self.View.E_Arrow_DownRightImage.Setalpha((ops & TODOperaType.DOWNRIGHT) != 0? enable : disable);
            self.View.E_Arrow_RightImage.Setalpha((ops & TODOperaType.RIGHT) != 0? enable : disable);
            self.View.E_Arrow_UpRightImage.Setalpha((ops & TODOperaType.UPRIGHT) != 0? enable : disable);
            self.View.E_Arrow_UpImage.Setalpha((ops & TODOperaType.UP) != 0? enable : disable);
            self.View.E_Arrow_UpLeftImage.Setalpha((ops & TODOperaType.UPLEFT) != 0? enable : disable);
            self.View.E_Arrow_LeftImage.Setalpha((ops & TODOperaType.LEFT) != 0? enable : disable);
            self.View.E_Arrow_DownLeftImage.Setalpha((ops & TODOperaType.DOWNLEFT) != 0? enable : disable);

            self.View.E_LightPunchImage.Setalpha((ops & TODOperaType.LIGHTPUNCH) != 0? enable : disable);
            self.View.E_LightKickImage.Setalpha((ops & TODOperaType.LIGHTKICK) != 0? enable : disable);
            self.View.E_MiddlePunchImage.Setalpha((ops & TODOperaType.MIDDLEPUNCH) != 0? enable : disable);
            self.View.E_MiddleKickImage.Setalpha((ops & TODOperaType.MIDDLEKICK) != 0? enable : disable);
            self.View.E_HeavyPunchImage.Setalpha((ops & TODOperaType.HEAVYPUNCH) != 0? enable : disable);
            self.View.E_HeavyKickImage.Setalpha((ops & TODOperaType.HEAVYKICK) != 0? enable : disable);
        }
    }
}