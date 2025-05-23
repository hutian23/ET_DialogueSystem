using System.Numerics;
using Box2DSharp.Collision.Shapes;
using ET.Event;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(b2Box))]
    public class HandleUpdateFlipCallback : AInvokeHandler<UpdateFlipCallback>
    {
        public override void Handle(UpdateFlipCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.instanceId) as b2Body;
            if (b2Body.GetFlip() == args.flip) return;

            //1. 更新朝向
            b2Body.flip = (FlipState)args.flip;

            foreach (long id in b2Body.b2BoxDict.Values)
            {
                b2Box b2Box = b2Body.GetChild<b2Box>(id);
                if((b2Box.GetBoxType() & Box2DHelper.HitboxMask) == 0) continue; // box类型不为hitbox
                
                //2. 销毁旧的夹具
                b2Body.DestroyFixture(b2Box.fixture);
                
                //3. 水平翻转夹具
                PolygonShape shape = new();
                shape.SetAsBox(b2Box.Size.X / 2f, b2Box.Size.Y / 2f, b2Box.Center * new Vector2(b2Body.GetFlip(), 1), 0);
                b2Box.fixtureDef.Shape = shape;
                b2Box.fixture = b2Body.CreateFixture(b2Box.fixtureDef);
            }
            
            //4. 更新显示层
            b2Body.SyncTrans();
        }
    }
}