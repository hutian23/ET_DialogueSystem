using System;
using Box2DSharp.Testbed.Unity;
using CommandLine;
using UnityEngine;

namespace ET
{
    public class Init: MonoBehaviour
    {
        private FixedUpdate fixedUpdate;

        private void Awake()
        {
            fixedUpdate = new FixedUpdate(TimeSpan.FromSeconds(1 / 60d), () => { EventSystem.Instance.FixedUpdate(); });
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            AppDomain.CurrentDomain.UnhandledException += (_, e) => { Log.Error(e.ExceptionObject.ToString()); };

            Game.AddSingleton<MainThreadSynchronizationContext>();

            // 命令行参数
            string[] args = "".Split(" ");
            Parser.Default.ParseArguments<Options>(args)
                    .WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
                    .WithParsed(Game.AddSingleton);

            Game.AddSingleton<TimeInfo>();
            Game.AddSingleton<Logger>().ILog = new UnityLogger();
            Game.AddSingleton<ObjectPool>();
            Game.AddSingleton<IdGenerater>();
            Game.AddSingleton<EventSystem>();
            Game.AddSingleton<TimerComponent>();
            Game.AddSingleton<CoroutineLockComponent>();

            ETTask.ExceptionHandler += Log.Error;

            Game.AddSingleton<CodeLoader>().Start();

            fixedUpdate.Start();
        }

        private void Update()
        {
            Game.Update();
            fixedUpdate.Update();
        }

        private void LateUpdate()
        {
            Game.LateUpdate();
            Game.FrameFinishUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Close();
        }
    }
}