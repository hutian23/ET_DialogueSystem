using System;
using System.Threading;
using CommandLine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
	public class Init: MonoBehaviour
	{
		[Button("Hello world")]
		public void Test()
		{
			Debug.LogWarning(Application.persistentDataPath);
		}
        
		private void Start()
		{
			DontDestroyOnLoad(gameObject);
			
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Log.Error(e.ExceptionObject.ToString());
			};
				
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
		}

		private void Update()
		{
			Game.Update();
		}

		private void FixedUpdate()
		{
			EventSystem.Instance.FixedUpdate();
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