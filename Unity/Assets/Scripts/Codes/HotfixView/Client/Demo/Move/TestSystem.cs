namespace ET.Client
{
    public static class TestSystem
    {
        public class TestAwakeSystem : AwakeSystem<Test>
        {
            protected override void Awake(Test self)
            {
                Log.Warning("Hello world");
            }
        }
        
        public class TestFixedUpdateSystem : FixedUpdateSystem<Test>
        {
            protected override void FixedUpdate(Test self)
            {
                Log.Warning("FixedUpdate");
            }
        }
    }
}