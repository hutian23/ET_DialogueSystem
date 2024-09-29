using System;
using Box2DSharp.Testbed.Unity;
using ET;
using MongoDB.Bson;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test: MonoBehaviour
{
    public AnimationCurve curve;

    [Button("test")]
    public void Test222()
    {
        var clone = MongoHelper.Clone(this.curve);
    }

    public float curPos;
    public int targetFrame;
    public FixedUpdate FixedUpdate;

    public void Start()
    {
        this.FixedUpdate = new FixedUpdate(TimeSpan.FromSeconds(1 / 60d), this.Tick);
        this.FixedUpdate.Start();
    }

    public void Tick()
    {
        this.targetFrame++;

        var pos = this.curve.Evaluate(this.targetFrame * (1 / 60f));
        var prePos = this.curve.Evaluate((this.targetFrame - 1) * (1 / 60f));
        var dv = pos - prePos;

        curPos += dv;
        Debug.LogWarning(this.curPos + "   " + pos);
    }

    public void Update()
    {
        this.FixedUpdate.Update();
    }
}