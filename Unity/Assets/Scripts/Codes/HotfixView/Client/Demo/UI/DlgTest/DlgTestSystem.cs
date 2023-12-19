namespace ET.Client
{
    [FriendOf(typeof (DlgTest))]
    public static class DlgTestSystem
    {
        public static void RegisterUIEvent(this DlgTest self)
        {
            self.View.E_LoginButton.AddListenerAsync(self.SceneChangeTo);
        }

        public class DlgTestLoadSystem: LoadSystem<DlgTest>
        {
            protected override void Load(DlgTest self)
            {
                self.View.E_LoginButton.AddListenerAsync(self.SceneChangeTo);
            }
        }

        public static void ShowWindow(this DlgTest self, Entity contextData = null)
        {
        }

        private static async ETTask SceneChangeTo(this DlgTest self)
        {
            await TODSceneChangeHelper.SceneChangeTo(self.ClientScene(), self.View.E_SceneInputField.text);
        }
    }
}