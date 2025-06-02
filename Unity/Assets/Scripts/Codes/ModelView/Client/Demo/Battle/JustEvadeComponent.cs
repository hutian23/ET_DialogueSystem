using System.Numerics;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class JustEvadeComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public long startFrame; // 记录从哪一帧启动窗口(SceneTimer)
        public int functionIndex;
        public int lastFrame; // 精准闪避窗口持续帧数
        public Vector2 boxSize; // 判定框数据
        public Vector2 boxOffset;
        public ETCancellationToken token;
    }
}