namespace ET.Client
{ 
    public class Trigger_CheckHP_BBScriptHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "HP";
        }
    }
//Trigger HP > 10: CallSubCoroutine func = PrintHelloWorld
//Trigger SP < 100: Invoke func = PrintHelloWorld    
//
//
//@PrintHelloWorld:
//LogWarning "Hello world";
//Trigger HP < 10: return; # 退出子携程
//
}