using Box2DSharp.Dynamics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    public class GroundCheckRayCastCallback: IRayCastCallback
    {
        public float RayCastCallback(Fixture fixture, in Vector2 point, in Vector2 normal, float fraction)
        {
            var body = fixture.Body;
            if (body.UserData is string && body.UserData.Equals("Ground"))
            {
                Debug.LogWarning("Hit");
                return 0f;
            }

            return -1.0f;
        }
    }
}