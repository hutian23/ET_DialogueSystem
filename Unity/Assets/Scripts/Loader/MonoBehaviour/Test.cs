using ET;
using MongoDB.Bson;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    // public AnimationCurve shakeCurve;
    // public float shakeDuration = 1f;
    // public float shakeIntensity = 0.1f;
    //
    // private Vector3 originalPosition;
    // private float shakeTimer;

    public AnimationCurve curve;

    [Button("test")]
    public void Test222()
    {
        var clone =MongoHelper.Clone(this.curve);
        Debug.LogWarning(clone.ToJson());
    }
    
    [TextArea]
    public string Text;
    
    void Start()
    {
        // originalPosition = transform.position;
    }

    void Update()
    {
        // if (shakeTimer > 0)
        // {
        //     // 计算震动的偏移量，根据时间和曲线来确定
        //     float shakeOffset = shakeIntensity * shakeCurve.Evaluate(1 - (shakeTimer / shakeDuration));
        //     
        //     // 生成随机的震动偏移
        //     Vector3 randomOffset = new Vector3(Random.Range(-shakeOffset, shakeOffset), Random.Range(-shakeOffset, shakeOffset), Random.Range(-shakeOffset, shakeOffset));
        //     
        //     // 将原始位置与随机偏移相加来设置新的位置
        //     transform.position = originalPosition + randomOffset;
        //
        //     // 减少计时器
        //     shakeTimer -= Time.deltaTime;
        // }
        // else
        // {
        //     // 重置位置
        //     transform.position = originalPosition;
        // }
        
    }

    // // 开始震动效果
    // [Button("开始震动")]
    // public void StartShake()
    // {
    //     shakeTimer = shakeDuration;
    // }
}