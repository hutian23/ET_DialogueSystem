using UnityEngine;

namespace ET.Client
{
    public class VN_Shake_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_Shake";
        }
        
        // VN_Shake effect = hold_it curve = ShakeCurve duration = ShakeDuration intensity = ShakeIntensity;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            
            GameObject effect = dialogueComponent.GetComponent<EffectManager>().GetEffect("hold_it");
            var startPos = effect.transform.position;
            float shakeTimer = 0f , duration = 0f;
            while (shakeTimer < duration)
            {
                if(token.IsCancel()) break;
                await TimerComponent.Instance.WaitFrameAsync(token);
            }
            await ETTask.CompletedTask;
        }
    }
}