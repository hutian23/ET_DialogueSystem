using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(CameraManager))]
    public class HandleShowGUICallback : AInvokeHandler<ShowGUICallback>
    {
        public override void Handle(ShowGUICallback args)
        {
            Global.Settings.ShowGUI = args.ShowGUI;
            CameraManager.instance.MainCamera.GetComponent<b2Game>().enabled = Global.Settings.ShowGUI;
        }
    }
}