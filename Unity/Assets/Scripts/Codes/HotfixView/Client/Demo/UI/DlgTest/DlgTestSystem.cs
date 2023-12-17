namespace ET.Client
{
    [FriendOf(typeof (DlgTest))]
    public static class DlgTestSystem
    {
        public static void RegisterUIEvent(this DlgTest self)
        {
            self.View.E_LoginButton.AddListenerAsync(self.SceneChangeTo);
        }

        public static void ShowWindow(this DlgTest self, Entity contextData = null)
        {
        }
        
        
        private static async ETTask SceneChangeTo(this DlgTest self)
        {
            // await TODSceneChangeHelper.SceneChangeTo(self.ClientScene(), self.View.E_SceneInputField.text.BundleNameToLower());
            Log.Warning("Hello World");
            Log.Warning("1111");
            await ETTask.CompletedTask;
        }
    }
}