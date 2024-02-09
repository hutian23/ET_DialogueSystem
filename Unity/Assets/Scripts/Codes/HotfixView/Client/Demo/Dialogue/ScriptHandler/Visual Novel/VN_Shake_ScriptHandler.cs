using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VN_Shake_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_Shake";
        }

        // VN_Shake effect = hold_it curve = ShakeCurve duration = ShakeDuration intensity = ShakeIntensity;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line,  @"VN_Shake effect = (?<effect>\w+) curve = (?<curve>\w+) duration = (?<duration>\w+) intensity = (?<intensity>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();

            GameObject effect = dialogueComponent.GetComponent<EffectManager>().GetEffect(match.Groups["effect"].Value);
            AnimationCurve shakeCurve = dialogueComponent.GetVariable<AnimationCurve>(match.Groups["curve"].Value);
            float duration = dialogueComponent.GetVariable<float>(match.Groups["duration"].Value);
            float intensity = dialogueComponent.GetVariable<float>(match.Groups["intensity"].Value);

            var originlPos = effect.transform.position;
            float shakeTimer = duration;

            while (shakeTimer > 0)
            {
                if (token.IsCancel()) break;
                
                // 计算震动的偏移量，根据时间和曲线来确定
                float shakeOffset = intensity * shakeCurve.Evaluate(1 - (shakeTimer / duration));
                
                // 随机生成震动偏移
                Vector3 randomOffset = new Vector3(Random.Range(-shakeOffset,shakeOffset), Random.Range(-shakeOffset, shakeOffset), Random.Range(-shakeOffset, shakeOffset));
                
                // 将原始位置与随机偏移相加来设置新的位置
                effect.transform.position = originlPos + randomOffset;
                
                // 减少计时器
                shakeTimer -= Time.deltaTime;
                await TimerComponent.Instance.WaitFrameAsync(token);
            }
            effect.transform.position = originlPos;
            await ETTask.CompletedTask;
        }
    }
}