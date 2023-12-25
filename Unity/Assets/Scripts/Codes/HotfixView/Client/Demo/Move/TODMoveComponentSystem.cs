using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (TODMoveComponent))]
    public static class TODMoveComponentSystem
    {
        public static TODMoveComponent Load(this TODMoveComponent self)
        {
            GameObject go = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;

            //初始化 碰撞体
            BoxCollider2D collider2D = go.GetComponent<BoxCollider2D>();
            var offset = collider2D.offset;
            var size = collider2D.size;
            self.Collider = new Rect(offset.x, offset.y, size.x, size.y);
            self.position = go.transform.position;

            return self;
        }

        public static TODMoveComponent RegisterAsActor(this TODMoveComponent self, ActorHandler handler)
        {
            self.IsActor = true;
            self.actorHandler = handler;
            return self;
        }

        public static TODMoveComponent RegisterAsSolid(this TODMoveComponent self, SolidHandler handler)
        {
            self.IsActor = false;
            self.solidHandler = handler;
            return self;
        }
        
        public static TODMoveComponent SetPosition(this TODMoveComponent self, Vector2 position)
        {
            self.GetTransform().position = position;
            return self;
        }

        //unit的物理碰撞箱
        public static TODMoveComponent SetCollider(this TODMoveComponent self, Rect collider)
        {
            BoxCollider2D collider2D = self.GetTransform().GetComponent<BoxCollider2D>();
            collider2D.offset = new Vector2(collider.x, collider.y);
            collider2D.size = collider.size;
            self.Collider = collider;
            return self;
        }

        private static Transform GetTransform(this TODMoveComponent self)
        {
            if (self == null || self.IsDisposed)
            {
                Log.Error("TODMovecomponent has been disposed");
                return null;
            }

            Unit unit = self.GetParent<Unit>();
            if (unit == null || unit.IsDisposed)
            {
                Log.Error("Parent unit has been disposed");
                return null;
            }

            if (unit.GetComponent<GameObjectComponent>() == null || unit.GetComponent<GameObjectComponent>().GameObject == null)
            {
                Log.Error($"Can't find gameObject in unit:{unit.Id}");
                return null;
            }

            return unit.GetComponent<GameObjectComponent>().GameObject.transform;
        }

        //当前位置
        public static Rect GetRect(this TODMoveComponent self)
        {
            return self.GetRect(self.position);
        }

        //预测位置
        private static Rect GetRect(this TODMoveComponent self, Vector2 tempPos)
        {
            //碰撞箱中心点
            Vector2 origion = tempPos + self.Collider.position;
            return new Rect(origion - self.Collider.size * 0.5f, self.Collider.size);
        }

        public static void EnableCollide(this TODMoveComponent self, bool enable)
        {
            self.GetTransform().GetComponent<BoxCollider2D>().enabled = enable;
        }
        
        // private static bool CheckGround(this TODMoveComponent self, Vector2 offset)
        // {
        //     Vector2 origion = self.position + self.Collider.position + offset;
        //     RaycastHit2D hit = Physics2D.BoxCast(origion, self.Collider.size, 0, Vector2.down, Constants.DEVIATION, Constants.GroundMask);
        //     //斜坡不能判断为地面
        //     if (hit && hit.normal == Vector2.up)
        //     {
        //         return true;
        //     }
        //
        //     return false;
        // }
        //
        // public static bool CheckGround(this TODMoveComponent self)
        // {
        //     return self.CheckGround(Vector2.zero);
        // }
        
        public static bool CollideCheck(this TODMoveComponent self, Vector2 position, Vector2 dir, LayerMask collideMask, float dist = 0)
        {
            Vector2 origion = position + self.Collider.position;
            return Physics2D.OverlapBox(origion + dir * (dist + Constants.DEVIATION),
                self.Collider.size,
                0,
                Constants.GroundMask);
        }
        
        public static bool CollideCheck(this TODMoveComponent self, Vector2 dir, LayerMask collideMask, float dist = 0)
        {
            return self.CollideCheck(self.position, dir, collideMask, dist);
        }
        
        public static (float, RaycastHit2D) MoveXStepWithCollide(this TODMoveComponent self, float distX)
        {
            Vector2 moved = Vector2.zero;
            //判断移动方向
            Vector2 direct = Mathf.Sign(distX) > 0? Vector2.right : Vector2.left;
            Vector2 origion = self.position + self.Collider.position;

            RaycastHit2D hit = Physics2D.BoxCast(origion,
                self.Collider.size,
                0,
                direct,
                Mathf.Abs(distX) + Constants.DEVIATION,
                self.CollideMask);

            // 请所有涉及碰撞的物体都设置为长方形!!! 如果是不规则、斜坡 都会判断为没发生碰撞
            if (hit && hit.normal == -direct)
            {
                //如果发生碰撞，则只能移动到碰撞物体附近
                moved += direct * Mathf.Max((hit.distance - Constants.DEVIATION), 0);
            }
            else
            {
                //没发生碰撞
                moved += Vector2.right * distX;
            }

            return (moved.x, hit);
        }
        
        public static (float, RaycastHit2D) MoveYStepWithCollide(this TODMoveComponent self, float distY)
        {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Mathf.Sign(distY) > 0? Vector2.up : Vector2.down;
            Vector2 origion = self.position + self.Collider.position;

            RaycastHit2D hit = Physics2D.BoxCast(origion,
                self.Collider.size,
                0,
                direct,
                Mathf.Abs(distY) + Constants.DEVIATION,
                self.CollideMask);

            if (hit && hit.normal == -direct)
            {
                moved += direct * Mathf.Max((hit.distance - Constants.DEVIATION), 0);
            }
            else
            {
                moved += Vector2.up * distY;
            }

            return (moved.y, hit);
        }
        
        public static bool CorrectX(this TODMoveComponent self, float distX)
        {
            Vector2 direct = Mathf.Sign(distX) > 0? Vector2.right : Vector2.left;
            //横向冲刺
            if (self.Speed.x != 0 && self.Speed.y == 0)
            {
                for (int i = 1; i <= Constants.DashCornerCorrection; i++)
                {
                    for (int j = 1; j >= -1; j -= 2)
                    {
                        Vector2 origion = self.position + new Vector2(0, j * i * Constants.STEP);
                        if (!self.CollideCheck(origion, direct, self.CollideMask, Mathf.Abs(distX)))
                        {
                            self.position += new Vector2(distX, j * i * Constants.STEP);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        public static bool CorrectY(this TODMoveComponent self, float distY)
        {
            Vector2 direct = Mathf.Sign(distY) > 0? Vector2.up : Vector2.down;
            //下落 不用修正y轴
            if (self.Speed.y < 0) return true;
            //横轴速度为0，则向左向右都尝试修正
            //尝试向左修正
            if (self.Speed.x <= 0)
            {
                for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                {
                    Vector2 origion = self.position + new Vector2(-i * Constants.STEP, 0);
                    if (!self.CollideCheck(origion, direct, self.CollideMask, Mathf.Abs(distY)))
                    {
                        self.position += new Vector2(-i * Constants.STEP, 0);
                        return true;
                    }
                }
            }

            //尝试向右修正
            if (self.Speed.x >= 0)
            {
                for (int i = 0; i <= Constants.UpwardCornerCorrection; i++)
                {
                    Vector2 origion = self.position + new Vector2(i * Constants.STEP, 0);
                    if (self.CollideCheck(origion, direct, self.CollideMask, Mathf.Abs(distY)))
                    {
                        self.position += new Vector2(i * Constants.STEP, 0);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}