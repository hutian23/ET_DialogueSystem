using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2WorldManager))]
    [FriendOf(typeof(BBParser))]
    public class CreateBox_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateBox";
        }

        //CreateBox: HitStop_1, 0, 0, 1000, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            
            //2. 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndCreateBox:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;

            //3. 夹具配置协程
            b2Body _body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            FixtureData fixtureData = new()
            {
                InstanceId = _body.InstanceId,
                Name = string.Empty,
                Type = FixtureType.Default,
                LayerMask = LayerType.Unit,
                IsTrigger = true,
                UserData = default,
                //事件
                TriggerEnterId = TriggerEnterType.SceneBoxEvent,
                TriggerStayId = TriggerStayType.SceneBoxEvent,
                TriggerExitId = TriggerExitType.SceneBoxEvent,
                CollisionEnterId = CollisionEnterType.SceneBoxEvent,
                CollisionStayId = CollisionStayType.SceneBoxEvent,
                CollisionExitId = CollisionExitType.SceneBoxEvent,
            };
            parser.RegistParam("FixtureData", fixtureData);
            parser.RegistParam("BoxInfo", new BoxInfo());
            await parser.RegistSubCoroutine(startIndex, endIndex, token);
            if (token.IsCancel()) return Status.Failed;
            
            //4. 新建夹具
            FixtureData _data = parser.GetParam<FixtureData>("FixtureData");
            
            BoxInfo _info = parser.GetParam<BoxInfo>("BoxInfo");
            _data.UserData = _info;

            PolygonShape _shape = new();
            _shape.SetAsBox(_info.size.x / 2f, _info.size.y / 2f, new Vector2(_info.center.x, _info.center.y), 0f);
            
            FixtureDef fixtureDef = new()
            {
                Shape = _shape, 
                Density = 1.0f,
                Friction = 0.0f,
                UserData = _data
            };
            _body.CreateFixture(fixtureDef);
            
            //5. 初始化
            parser.TryRemoveParam("FixtureDef");
            parser.TryRemoveParam("BoxInfo");
            
            return Status.Success;
        }
    }
}