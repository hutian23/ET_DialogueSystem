namespace ET.Client
{
    public static class ScriptHelper
    {
        public static void ScriptMatchError(string text)
        {
            Log.Error($"{text}匹配失败，请检查格式!!!");
        }
    }
}