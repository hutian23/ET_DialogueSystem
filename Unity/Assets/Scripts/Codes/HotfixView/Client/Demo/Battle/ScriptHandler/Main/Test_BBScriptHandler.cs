using MongoDB.Bson;
using UnityEngine;

namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.AirDashToGroundComponent))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}