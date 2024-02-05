namespace ET.Client
{
    public static class TODAIHelper
    {
        public static bool AI_ContainBuff<T>(this Unit unit) where T : Entity
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning($"unit is null!!");
                return false;
            }

            TODAIComponent aiComponent = unit.GetComponent<TODAIComponent>();
            if (aiComponent == null || aiComponent.GetComponent<Buffer>() == null)
            {
                Log.Warning($"please add buffer to unit! :{unit.InstanceId}");
                return false;
            }

            return aiComponent.GetComponent<Buffer>().GetComponent<T>() == null;
        }

        public static void AI_RemoveBuff<T>(this Unit unit) where T : Entity
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning($"unit is null!!");
                return;
            }

            TODAIComponent aiComponent = unit.GetComponent<TODAIComponent>();
            aiComponent?.GetComponent<Buffer>()?.RemoveComponent<T>();
        }

        public static T AI_GetBuff<T>(this Unit unit) where T : Entity
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning($"unit is null!!!");
                return null;
            }

            TODAIComponent aiComponent = unit.GetComponent<TODAIComponent>();
            return aiComponent?.GetComponent<Buffer>()?.GetComponent<T>();
        }

        public static T AI_AddBuff<T>(this Unit unit) where T : Entity, IAwake, new()
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning("unit is null");
                return null;
            }

            if (unit.GetComponent<TODAIComponent>() == null || unit.GetComponent<TODAIComponent>().GetComponent<Buffer>() == null)
            {
                Log.Warning($"please add buffer to unit!!: {unit.InstanceId}");
                return null;
            }

            return unit.AI_GetBuff<T>() != null? unit.AI_GetBuff<T>() : unit.GetComponent<TODAIComponent>().GetComponent<Buffer>().AddComponent<T>();
        }

        public static void AnimPlay(this Unit unit, string clipName)
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning("unit is null");
                return;
            }

            AnimatorComponent anim = unit.GetComponent<AnimatorComponent>();
            if (anim == null)
            {
                Log.Warning($"please add animcomponent to unit!!: {unit.InstanceId}");
                return;
            }

            anim.Play(clipName);
        }

        public static void AnimPlay_Repeat(this Unit unit, string clipName)
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning("unit is null");
                return;
            }

            AnimatorComponent anim = unit.GetComponent<AnimatorComponent>();
            if (anim == null)
            {
                Log.Warning($"please add animcomponent to unit!!: {unit.InstanceId}");
                return;
            }
            anim.Play_Repeat(clipName);
        }
        
        public static async ETTask AnimPlayCor(this Unit unit, SubBehavior subBehavior, ETCancellationToken token)
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning("unit is null");
                return;
            }

            AnimatorComponent anim = unit.GetComponent<AnimatorComponent>();
            if (anim == null)
            {
                Log.Warning($"please add animComponent to unit!!: {unit.InstanceId}");
                return;
            }

            anim.Play(subBehavior.ClipName);
            await unit.WaitAsync(subBehavior.frame, token);
        }
    }
}